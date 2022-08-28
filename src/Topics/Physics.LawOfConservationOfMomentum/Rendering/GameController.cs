using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.LawOfConservationOfMomentum.Game;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.LawOfConservationOfMomentum.Rendering
{
	public class GameController : SkiaCanvasController
	{
		private SKBitmap _backgroundBitmap = null;
		private SKBitmap _objectBitmap = null;
		
		private SKBitmap _netBitmap = null;
		private SKBitmap _netHandleBitmap = null;

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

			DrawNet(args);
		}

		public override void Update(ISkiaCanvas sender)
		{
			EnsureBitmaps();
			//if (GameInfo is null)
			//{
			//	return;
			//}

			//if (PreviewPhysicsService is null ||
			//	PreviewPhysicsService.Setup.InclinedAngle != GameInfo.CurrentAngle)
			//{
			//	PreviewPhysicsService = new PhysicsService(GameInfo.CreateGameMotionSetupWithAngle(GameInfo.CurrentAngle));
			//}
		}

		private void DrawNet(SKSurface args)
		{
			var netY = GameInfo.PhysicsService.CalculateNetY((float)SimulationTime.TotalTime.TotalSeconds);

			var netCenterY = 585f;
			netCenterY = netY + 585;
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
				_objectBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Object.png"));
				_netBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "net.png"));
				_netHandleBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "nethandle.png"));
			}
		}
	}
}
