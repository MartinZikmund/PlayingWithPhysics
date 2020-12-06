using System;
using System.Numerics;
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
		private const int HelicopterHeight = 15;
		private const int RaftWidth = 40;
		private const int UnderwaterDepth = 10;
		private const int CargoWidth = 10;

		private readonly Random _randomizer = new Random();

		private readonly ISoundPlayer _soundPlayer;

		private CanvasBitmap _backgroundImage;
		private CanvasBitmap[] _helicopterImages;
		private CanvasBitmap _raftImage;
		private CanvasBitmap _paraschuteFullOpen;
		private double _pixelsPerMeter = 1;

		private GameInfo _game = null;
		private RaftPhysicsService _raftPhysicsService = null;
		private MotionInfo _dropMotion;
		private PhysicsService _dropPhysicsService;

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
				DrawHelicopter(sender, args);

				DrawRaft(sender, args);

				if (_game.State == GameState.Dropped)
				{
					DrawCargo(sender, args);
				}

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
			_game.SetState(GameState.Dropped);
			_game.DropTime = SimulationTime.TotalTime;
			_dropMotion = MotionFactory.CreateFreeFall(
				new Vector2(0, (float)_game.HelicopterAltitude),
				1.3f,
				_game.CargoMass,
				1,
				0,
				0,
				9.81f,
				1.3f,
				0f,
				0f,
				"#000000");
			_dropPhysicsService = new PhysicsService(_dropMotion);
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

			args.DrawingSession.DrawImage(_helicopterImages[spriteIndex], new Rect(left, helicopterHeight + swivel, actualWidth, actualHeight));
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

			var top = (WorldHeight - UnderwaterDepth) * _pixelsPerMeter - actualHeight;

			args.DrawingSession.DrawImage(_raftImage, new Rect(left, top, actualWidth, actualHeight));
		}

		private void DrawCargo(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
		{
			if (_dropPhysicsService != null)
			{
				var dropTime = SimulationTime.TotalTime - _game.DropTime;
				var y = _dropPhysicsService.ComputeY((float)dropTime.TotalSeconds);
				var actualWidth = _pixelsPerMeter * CargoWidth;
				var scale = actualWidth / _paraschuteFullOpen.Size.Width;

				var actualHeight = _paraschuteFullOpen.Size.Height * scale;

				var left = sender.Size.Width / 2 - actualWidth / 2;

				var top = (WorldHeight - GetAltitudeInWorld(y)) * _pixelsPerMeter - actualHeight;

				args.DrawingSession.DrawImage(_paraschuteFullOpen, new Rect(left, top, actualWidth, actualHeight));
			}
		}

		public override async Task CreateResourcesAsync(CanvasAnimatedControl sender)
		{
			await base.CreateResourcesAsync(sender);

			await _soundPlayer.PreloadSoundAsync(new Uri("ms-appx:///Assets/Game/Splash.wav", UriKind.Absolute), "Splash");

			_backgroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/jezero.jpg"));
			_helicopterImages = new[]{
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vrtulnik1.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vrtulnik2.png")),
				await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vrtulnik3.png"))
			};
			_raftImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/vor.png"));
			_paraschuteFullOpen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Game/padak-01.png"));
		}

		private double GetAltitudeInWorld(double altitude)
		{
			return altitude + UnderwaterDepth;
		}
	}
}
