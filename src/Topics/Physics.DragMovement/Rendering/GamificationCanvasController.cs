using System;
using System.Numerics;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.DragMovement.Gamification;
using Physics.DragMovement.Logic.PhysicsServices;
using Physics.Shared.Services.Sounds;
using Windows.Foundation;

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

		private const int ParaschuteHalfOpenTime = 2;
		private const int ParaschuteFullOpenTime = 5;

		private readonly Random _randomizer = new Random();

		private readonly ISoundPlayer _soundPlayer;

		private SerialDisposable _helicopterSoundPlayback = new SerialDisposable();

		private CanvasBitmap _backgroundImage;
		private CanvasBitmap[] _helicopterImages;
		private CanvasBitmap _raftImage;
		private CanvasBitmap[] _paraschuteImages;
		private double _pixelsPerMeter = 1;

		private GameInfo _game = null;
		private RaftPhysicsService _raftPhysicsService = null;
		private MotionInfo _closedParachuteDropInfo;
		private MotionInfo _halfOpenParachuteDropInfo;
		private MotionInfo _fullOpenParachuteDropInfo;
		private PhysicsService _closedParachutePhysicsService;
		private PhysicsService _halfOpenParachutePhysicsService;
		private PhysicsService _fullOpenParachutePhysicsService;

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
							_soundPlayer.PlaySound("Thud", 0.5);
						}
						else
						{
							_game.SetState(GameState.Missed);
							_soundPlayer.PlaySound("Splash", 1);
						}
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

				if (_game.State == GameState.Dropped || _game.State == GameState.Won)
				{
					if (SimulationTime.TotalTime < _game.HitTime)
					{
						DrawCargo(sender, args);
					}
				}

				DrawHelicopter(sender, args);

				DrawOverlay(sender, args);
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
			if (_helicopterSoundPlayback.Disposable == null)
			{
				_helicopterSoundPlayback.Disposable = _soundPlayer.PlayIndefinitely("Heli", 0.5);
			}
			Reset();
			Play();
		}

		internal void RestartAttempt()
		{
			_game.SetState(GameState.NotStarted);
			Reset();
			Pause();
		}

		internal void DropCargo()
		{
			_game.DropTime = SimulationTime.TotalTime;
			_closedParachuteDropInfo = MotionFactory.CreateFreeFall(
				new Vector2(0, (float)_game.HelicopterAltitude),
				1.3f,
				_game.CargoMass,
				0.1f,
				0,
				0,
				9.81f,
				1.3f,
				0f,
				0f,
				"#000000");
			_closedParachutePhysicsService = new PhysicsService(_closedParachuteDropInfo);
			var halfOpenHeight = _closedParachutePhysicsService.ComputeY(ParaschuteHalfOpenTime);
			_halfOpenParachuteDropInfo = MotionFactory.CreateFreeFall(
				new Vector2(0, (float)halfOpenHeight),
				1.3f,
				_game.CargoMass,
				0.5f,
				0,
				0,
				9.81f,
				1.3f,
				0f,
				0f,
				"#000000");
			_halfOpenParachutePhysicsService = new PhysicsService(_halfOpenParachuteDropInfo);
			var fullOpenHeight = _halfOpenParachutePhysicsService.ComputeY(ParaschuteFullOpenTime - ParaschuteHalfOpenTime);
			_fullOpenParachuteDropInfo = MotionFactory.CreateFreeFall(
				new Vector2(0, (float)fullOpenHeight),
				1.3f,
				_game.CargoMass,
				1f,
				0,
				0,
				9.81f,
				1.3f,
				0f,
				0f,
				"#000000");
			_fullOpenParachutePhysicsService = new PhysicsService(_fullOpenParachuteDropInfo);

			var hitTime = _fullOpenParachutePhysicsService.MaxT + ParaschuteFullOpenTime;
			_game.HitTime = TimeSpan.FromSeconds(_game.DropTime.TotalSeconds + hitTime);
			var raftPosition = _raftPhysicsService.GetX(_game.HitTime.TotalSeconds);
			_game.WillDropOnRaft =
				raftPosition >= (WorldWidth / 2 - RaftWidth / 2) &&
				raftPosition <= (WorldWidth / 2 + RaftWidth / 2);
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
			if (_closedParachutePhysicsService != null)
			{
				var dropTime = SimulationTime.TotalTime - _game.DropTime;
				CanvasBitmap image;
				float y;
				if (dropTime.TotalSeconds <= ParaschuteHalfOpenTime)
				{
					image = _paraschuteImages[2];
					y = _closedParachutePhysicsService.ComputeY((float)dropTime.TotalSeconds);
				}
				else if (dropTime.TotalSeconds <= ParaschuteFullOpenTime)
				{
					image = _paraschuteImages[1];
					y = _halfOpenParachutePhysicsService.ComputeY((float)dropTime.TotalSeconds - ParaschuteHalfOpenTime);
				}
				else
				{
					image = _paraschuteImages[0];
					y = _fullOpenParachutePhysicsService.ComputeY((float)dropTime.TotalSeconds - ParaschuteFullOpenTime);
				}

				var actualWidth = _pixelsPerMeter * CargoWidth;
				var scale = actualWidth / _paraschuteImages[0].Size.Width;

				var actualHeight = _paraschuteImages[0].Size.Height * scale;

				var left = sender.Size.Width / 2 - actualWidth / 2;

				var top = (WorldHeight - GetAltitudeInWorld(y)) * _pixelsPerMeter - actualHeight;

				args.DrawingSession.DrawImage(image, new Rect(left, top, actualWidth, actualHeight), new Rect(0, 0, _paraschuteImages[0].Size.Width, _paraschuteImages[0].Size.Height), 1, CanvasImageInterpolation.MultiSampleLinear);
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
			_paraschuteImages = new[]
			{
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak-01.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak-02.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak-03.png")),
			};
		}

		private double GetAltitudeInWorld(double altitude)
		{
			return altitude + UnderwaterDepth;
		}

		private double GetLeftHorizontalBound(ICanvasAnimatedControl sender)
		{
			return sender.Size.Width / 2 - WorldWidth / 2 * _pixelsPerMeter;
		}

		public override void Dispose()
		{
			base.Dispose();
			_helicopterSoundPlayback.Disposable = null;
		}
	}
}
