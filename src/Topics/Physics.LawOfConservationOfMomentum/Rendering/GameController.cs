using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public GameController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		//internal GamePhysicsService PhysicsService;

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.Clear(new SKColor(255, 244, 244, 244));

			//if (GameInfo is null)
			//{
			//	return;
			//}

			DrawBackground(sender, args);
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
			}
		}
	}
}
