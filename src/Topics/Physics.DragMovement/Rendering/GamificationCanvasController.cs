using System;
using System.Linq;
using System.Numerics;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.DragMovement.Gamification;
using Physics.DragMovement.Logic.PhysicsServices;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Localization;
using Windows.Foundation;
using Windows.UI;

namespace Physics.DragMovement.Rendering
{
	public class GamificationCanvasController : DragMovementCanvasController
	{
		private const int WorldHeight = 220;
		private const int WorldWidth = 220;
		private const int HelicopterHeight = 15;
		private const int RaftWidth = 40;
		private const int UnderwaterDepth = 10;
		private const int CargoWidth = 10;

		private const float CargoArea = 2;

		private readonly float[] _parachuteOpenTimes = new float[]
		{
			//0 - 1 - use image 0, 
			1,
			2,
			3,
			4,
			5,
			float.MaxValue
			//5 - maxT - use image 5
		};

		private CanvasTextFormat _gameOverTextFormat = new CanvasTextFormat()
		{
			FontSize = 40,
			FontWeight = new Windows.UI.Text.FontWeight() { Weight = 400 },
			HorizontalAlignment = CanvasHorizontalAlignment.Center,
		};

		private readonly Random _randomizer = new Random();

		private readonly ISoundPlayer _soundPlayer;

		private SerialDisposable _helicopterSoundPlayback = new SerialDisposable();

		private CanvasBitmap _backgroundImage;
		private CanvasBitmap[] _helicopterImages;
		private CanvasBitmap _raftImage;
		private CanvasBitmap[] _parachuteImages;
		private double _pixelsPerMeter = 1;

		private GameInfo _game = null;
		private RaftPhysicsService _raftPhysicsService = null;

		private MotionInfo[] _parachuteStateMotionInfos = null;
		private PhysicsService[] _parachutePhysicsServices = null;

		public GamificationCanvasController(CanvasAnimatedControl canvasAnimatedControl, ISoundPlayer soundPlayer) : base(canvasAnimatedControl)
		{
			_soundPlayer = soundPlayer;
		}

		internal async Task StartNewGameAsync(GameInfo game)
		{
			await RunOnGameLoopAsync(() =>
			{
				_game = game;
				_motions = Array.Empty<MotionInfo>();
				_trajectories = Array.Empty<TrajectoryData>();
				_raftPhysicsService = new RaftPhysicsService(game);

				_parachuteStateMotionInfos = null;
				_parachutePhysicsServices = null;
				Pause();
				CalculateMaxima();
			});
		}

		public override void Update(ICanvasAnimatedControl sender)
		{
			base.Update(sender);
			if (_game != null)
			{
				if (_game.State == GameState.Dropped)
				{
					if (SimulationTime.TotalTime >= _game.HitTime)
					{
						if (_game.WillDropOnRaft)
						{
							_game.SetState(GameState.Won);
							if (_game.AreSoundsEnabled)
							{
								_soundPlayer.PlaySound("Thud", 0.5);
							}
						}
						else
						{
							_game.SetState(GameState.Missed);
							if (_game.AreSoundsEnabled)
							{
								_soundPlayer.PlaySound("Splash", 1);
							}
						}
					}
				}
				if (_game.State >= GameState.Started)
				{
					if (_game.AreSoundsEnabled && _helicopterSoundPlayback.Disposable == null)
					{
						_helicopterSoundPlayback.Disposable = _soundPlayer.PlayIndefinitely("Heli", 0.3);
					}

					if (!_game.AreSoundsEnabled && _helicopterSoundPlayback.Disposable != null)
					{
						_helicopterSoundPlayback.Disposable = null;
					}
				}
			}
		}

		public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			_pixelsPerMeter = sender.Size.Height / WorldHeight;

			if (_pixelsPerMeter < 0.5)
			{
				return;
			}

			args.DrawingSession.Antialiasing = CanvasAntialiasing.Antialiased;

			DrawBackground(sender, args);

			if (_game != null)
			{
				DrawRaft(sender, args);

				if ((_game.State == GameState.Dropped && SimulationTime.TotalTime < _game.HitTime) || _game.State == GameState.Won)
				{
					DrawCargo(sender, args);
				}

				DrawHelicopter(sender, args);

				DrawOverlay(sender, args);

				DrawGameOverText(sender, args);
			}
		}

		protected override void DrawBackground(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 68, 197, 214));

			var actualWidth = sender.Size.Width;

			var scale = actualWidth / _backgroundImage.Size.Width;
			var actualHeight = scale * _backgroundImage.Size.Height;

			args.DrawingSession.DrawImage(_backgroundImage, new Rect(0, sender.Size.Height - actualHeight, actualWidth, actualHeight));
		}

		internal void StartAttempt()
		{
			if (_game == null)
			{
				return;
			}

			_game.SetState(GameState.Started);
			Reset();
			Play();
		}

		internal void RestartAttempt()
		{
			_game.SetState(GameState.NotStarted);
			StopHeliSound();
			Reset();
			Pause();
		}

		internal void DropCargo()
		{
			_game.DropTime = SimulationTime.TotalTime;

			_parachuteStateMotionInfos = new MotionInfo[_parachuteOpenTimes.Length];
			_parachutePhysicsServices = new PhysicsService[_parachuteOpenTimes.Length];

			var partialArea = CargoArea / _parachuteOpenTimes.Length;
			var currentHeight = (float)_game.HelicopterAltitude;
			var startSpeed = 0f;
			var previousTime = 0f;
			for (int parachuteStateId = 0; parachuteStateId < _parachuteOpenTimes.Length; parachuteStateId++)
			{
				var currentArea = (parachuteStateId + 1) * partialArea;
				var motionInfo = MotionFactory.CreateProjectileMotion(
					new Vector2(0, currentHeight),
					1.3f,
					_game.CargoMass,
					currentArea,
					startSpeed,
					-90,
					9.81f,
					1.3f,
					0f,
					0f,
					"#000000");
				_parachuteStateMotionInfos[parachuteStateId] = motionInfo;
				var physicsService = new PhysicsService(motionInfo);
				_parachutePhysicsServices[parachuteStateId] = physicsService;
				var fallTime = _parachuteOpenTimes[parachuteStateId] - previousTime;
				currentHeight = physicsService.ComputeY(fallTime);
				startSpeed = physicsService.ComputeV(fallTime);
				previousTime = _parachuteOpenTimes[parachuteStateId];
			}

			var hitTime = _parachutePhysicsServices[_parachutePhysicsServices.Length - 1].MaxT + _parachuteOpenTimes[_parachuteOpenTimes.Length - 2];
			_game.HitTime = TimeSpan.FromSeconds(_game.DropTime.TotalSeconds + hitTime);
			var raftPosition = _raftPhysicsService.GetX(_game.HitTime.TotalSeconds);
			_game.WillDropOnRaft =
				raftPosition >= (WorldWidth / 2 - RaftWidth * 0.36) &&
				raftPosition <= (WorldWidth / 2 + RaftWidth * 0.36);
			_game.SetState(GameState.Dropped);
		}

		private void DrawHelicopter(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			var actualHeight = _pixelsPerMeter * HelicopterHeight;
			var scale = actualHeight / _helicopterImages[0].Size.Height;

			var actualWidth = _helicopterImages[0].Size.Width * scale;

			var centerX = sender.Size.Width / 2;
			var left = centerX - actualWidth / 2;

			var secondState = args.Timing.TotalTime.TotalSeconds - Math.Truncate(args.Timing.TotalTime.TotalSeconds);
			if (secondState > 0.5)
			{
				secondState = 1 - secondState;
			}
			var swivel = -(secondState / 0.5) * 4;

			var helicopterHeight = (WorldHeight - GetAltitudeInWorld(_game.HelicopterAltitude)) * _pixelsPerMeter - actualHeight;

			var secondPart = (int)(args.Timing.TotalTime.TotalSeconds / 0.03);
			var spriteIndex = secondPart % _helicopterImages.Length;

			args.DrawingSession.DrawImage(_helicopterImages[spriteIndex], new Rect(left, helicopterHeight + swivel, actualWidth, actualHeight), new Rect(0, 0, _helicopterImages[0].Size.Width, _helicopterImages[0].Size.Height), 1, CanvasImageInterpolation.MultiSampleLinear);
		}

		private void DrawRaft(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			if (_raftPhysicsService == null)
			{
				return;
			}

			var actualWidth = _pixelsPerMeter * RaftWidth;
			var scale = actualWidth / _raftImage.Size.Width;

			var actualHeight = _helicopterImages[0].Size.Height * scale;

			var centerX = _raftPhysicsService.GetX(SimulationTime.TotalTime.TotalSeconds);
			var leftInMeters = centerX - RaftWidth / 2;
			var left = leftInMeters * _pixelsPerMeter;

			var top = (WorldHeight - UnderwaterDepth + 3f) * _pixelsPerMeter - actualHeight;

			args.DrawingSession.DrawImage(_raftImage, new Rect(GetLeftHorizontalBound(sender) + left, top, actualWidth, actualHeight));
		}

		private void DrawCargo(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			if (_parachutePhysicsServices != null)
			{
				var fallTime = SimulationTime.TotalTime - _game.DropTime;
				CanvasBitmap image;
				float y;

				var firstHigherIndex = 0;
				for (int openTimeId = 0; openTimeId < _parachuteOpenTimes.Length; openTimeId++)
				{
					if (fallTime.TotalSeconds < _parachuteOpenTimes[openTimeId])
					{
						firstHigherIndex = openTimeId;
						break;
					}
				}

				var stageTime = fallTime.TotalSeconds;
				if (firstHigherIndex > 0)
				{
					//subtract previous open time to calculate the actual time in this stage
					stageTime -= _parachuteOpenTimes[firstHigherIndex - 1];
				}

				image = _parachuteImages[firstHigherIndex];

				var physicsService = _parachutePhysicsServices[firstHigherIndex];
				
				y = physicsService.ComputeY((float)stageTime);

				var actualWidth = _pixelsPerMeter * CargoWidth;
				var scale = actualWidth / _parachuteImages[0].Size.Width;

				var actualHeight = _parachuteImages[0].Size.Height * scale;

				var left = sender.Size.Width / 2 - actualWidth / 2;

				if (_game.State == GameState.Won)
				{
					// move cargo with raft
					var addX = _raftPhysicsService.GetX(SimulationTime.TotalTime.TotalSeconds) - _raftPhysicsService.GetX(_game.HitTime.TotalSeconds);
					left += addX * _pixelsPerMeter;
				}

				var top = (WorldHeight - GetAltitudeInWorld(y)) * _pixelsPerMeter - actualHeight;

				args.DrawingSession.DrawImage(image, new Rect(left, top, actualWidth, actualHeight), new Rect(0, 0, _parachuteImages[0].Size.Width, _parachuteImages[0].Size.Height), 1, CanvasImageInterpolation.MultiSampleLinear);
			}
		}

		public override async Task CreateResourcesAsync(CanvasAnimatedControl sender)
		{
			await base.CreateResourcesAsync(sender);

			await _soundPlayer.PreloadSoundAsync(new Uri("ms-appx:///Assets/Game/Splash.wav", UriKind.Absolute), "Splash");
			await _soundPlayer.PreloadSoundAsync(new Uri("ms-appx:///Assets/Game/Thud.wav", UriKind.Absolute), "Thud");
			await _soundPlayer.PreloadSoundAsync(new Uri("ms-appx:///Assets/Game/Heli.mp3", UriKind.Absolute), "Heli");

			_backgroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/jezero.jpg"));
			_helicopterImages = new[]{
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vrtulnik1.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vrtulnik2.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vrtulnik3.png"))
			};
			_raftImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vor.png"));
			_parachuteImages = new[]
			{
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak_1.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak_2.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak_3.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak_4.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak_5.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak_6.png")),
			};
		}

		private void DrawGameOverText(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			if (_game.State == GameState.Won)
			{
				args.DrawingSession.DrawText(Localizer.Instance.GetString("YouWin"), new Vector2((float)sender.Size.Width / 2, (float)sender.Size.Height / 2), Colors.DarkGreen, _gameOverTextFormat);
			}
			else if (_game.State == GameState.Missed)
			{
				args.DrawingSession.DrawText(Localizer.Instance.GetString("Missed"), new Vector2((float)sender.Size.Width / 2, (float)sender.Size.Height / 2), Colors.Red, _gameOverTextFormat);
			}
		}

		private double GetAltitudeInWorld(double altitude)
		{
			return altitude + UnderwaterDepth;
		}

		private double GetLeftHorizontalBound(ICanvasAnimatedControl sender)
		{
			return sender.Size.Width / 2 - WorldWidth / 2 * _pixelsPerMeter;
		}

		private void StopHeliSound()
		{
			_helicopterSoundPlayback.Disposable = null;
		}

		public override void Dispose()
		{
			base.Dispose();
			_helicopterSoundPlayback.Disposable = null;
		}
	}
}
