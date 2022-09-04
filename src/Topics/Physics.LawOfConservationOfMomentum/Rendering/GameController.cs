using System;
using System.IO;
using Physics.LawOfConservationOfMomentum.Game;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.LawOfConservationOfMomentum.Rendering
{
	public class GameController : SkiaCanvasController
	{
		private SKBitmap _backgroundBitmap = null;

		private SKBitmap _netBitmap = null;
		private SKBitmap _netHandleBitmap = null;
		private SKBitmap _ballBitmap = null;
		private SKBitmap _baronBitmap = null;
		private SKBitmap _canonBitmap = null;

		private const float PixelsPerMeter = 91f;
		private const float CenterLineY = 555f;
		private const float CanonEndX = 274f;
		
		public GameController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public GameInfo GameInfo { get; private set; }

		public void StartGame(GameInfo gameInfo)
		{
			if (gameInfo is null)
			{
				throw new ArgumentNullException(nameof(gameInfo));
			}

			GameInfo = gameInfo;
			Play();
		}

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));

			DrawBackground(sender, args);

			if (GameInfo is null)
			{
				return;
			}

			DrawCanon(args);
			DrawBall(args);
			DrawBaron(args);
			DrawNet(args);
		}

		public override void Update(ISkiaCanvas sender)
		{
			EnsureBitmaps();
			if (GameInfo?.AttemptPhysicsService is { } physicsService && GameInfo.State == GameState.Fired)
			{
				var endTime = physicsService.CalculateEndTime();
				var fireTime = GameInfo.CurrentFireTime;
				var currentTime = (float)SimulationTime.TotalTime.TotalSeconds;
				var attemptTime = currentTime - fireTime;
				if (attemptTime >= endTime)
				{
					GameInfo.SimulationEnded();
				}
			}

			//if (PreviewPhysicsService is null ||
			//	PreviewPhysicsService.Setup.InclinedAngle != GameInfo.CurrentAngle)
			//{
			//	PreviewPhysicsService = new PhysicsService(GameInfo.CreateGameMotionSetupWithAngle(GameInfo.CurrentAngle));
			//}
		}

		private void DrawNet(SKSurface args)
		{
			var netYMeters = 0f;
			if (GameInfo.AttemptPhysicsService is not null)
			{
				netYMeters = GameInfo.AttemptPhysicsService.CalculateNetY((float)SimulationTime.TotalTime.TotalSeconds);
			}
			else
			{
				netYMeters = GameInfo.PhysicsService.CalculateNetY((float)SimulationTime.TotalTime.TotalSeconds);
			}

			var netCenterY = netYMeters * PixelsPerMeter + CenterLineY;
			var netX = 1512;
			var netBimtapXOffset = 20;
			var netBitmapX = netX - netBimtapXOffset;
			var netHandleX = netX - 4;

			args.Canvas.DrawBitmap(
				_netBitmap,
				new SKRect(0, 0, _netBitmap.Width, _netBitmap.Height),
				new SKRect(netBitmapX, netCenterY - _netBitmap.Height / 2, netBitmapX + _netBitmap.Width, netCenterY + _netBitmap.Height / 2));

			args.Canvas.DrawBitmap(
				_netHandleBitmap,
				new SKRect(0, 0, _netHandleBitmap.Width, _netHandleBitmap.Height),
				new SKRect(netHandleX, netCenterY + 100, netHandleX + _netHandleBitmap.Width, 860));
		}

		private void DrawBall(SKSurface args)
		{
			if (GameInfo.AttemptPhysicsService is null)
			{
				return;
			}

			var ballX = GameInfo.AttemptPhysicsService.CalculateBallX((float)SimulationTime.TotalTime.TotalSeconds) * PixelsPerMeter + CanonEndX;
			var ballY = CenterLineY + 48;
			args.Canvas.DrawBitmap(
				_ballBitmap,
				new SKRect(0, 0, _ballBitmap.Width, _ballBitmap.Height),
				new SKRect(ballX - _ballBitmap.Width, ballY - _ballBitmap.Height / 2, ballX, ballY + _ballBitmap.Height / 2));
		}

		private void DrawCanon(SKSurface args)
		{
			var canonX = 100;
			var canonY = 628;
			args.Canvas.DrawBitmap(
				_canonBitmap,
				new SKRect(0, 0, _canonBitmap.Width, _canonBitmap.Height),
				new SKRect(canonX, canonY - _canonBitmap.Height / 2, canonX + _canonBitmap.Width, canonY + _canonBitmap.Height / 2));
		}

		private void DrawBaron(SKSurface args)
		{
			var baronX = 0f;
			if (GameInfo.AttemptPhysicsService is not null)
			{
				baronX = GameInfo.AttemptPhysicsService.CalculateBaronX((float)SimulationTime.TotalTime.TotalSeconds);
			}
			else
			{
				baronX = GameAttemptPhysicsService.CollisionX;
			}

			baronX *= PixelsPerMeter;
			baronX += CanonEndX;

			var baronY = CenterLineY;
			args.Canvas.DrawBitmap(
				_baronBitmap,
				new SKRect(0, 0, _baronBitmap.Width, _baronBitmap.Height),
				new SKRect(baronX - _baronBitmap.Width / 2, baronY - _baronBitmap.Height / 2, baronX + _baronBitmap.Width / 2, baronY + _baronBitmap.Height / 2));
		}

		private void DrawBackground(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.DrawBitmap(
				_backgroundBitmap,
				new SKRect(0, 0, _backgroundBitmap.Width, _backgroundBitmap.Height),
				new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height));
		}

		private void EnsureBitmaps()
		{
			if (_backgroundBitmap == null)
			{
				var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Game");
				_backgroundBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "celek.png"));
				_netBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "net.png"));
				_netHandleBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "nethandle.png"));
				_ballBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "ball.png"));
				_baronBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "baron.png"));
				_canonBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "canon.png"));
			}
		}
	}
}
