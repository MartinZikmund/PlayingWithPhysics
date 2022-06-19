using System.IO;
using Physics.OpticalInstruments.Game;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.OpticalInstruments.Rendering
{
	public class GameCanvasController : SkiaCanvasController
	{
		private SKBitmap _backgroundBitmap;
		private SKBitmap _mirrorBitmap;
		private SKBitmap _planetBitmap;
		private SKBitmap _frontLegBitmap;

		public GameCanvasController(ISkiaCanvas control) : base(control)
		{
		}

		public GameInfo GameInfo { get; internal set; }

		public override void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (GameInfo == null)
			{
				return;
			}
			DrawBackground(sender, args);
			DrawMirror(sender, args);
			if (GameInfo.State != GameState.SetAngle)
			{
				DrawLaser(sender, args);
			}
		}

		public override void Update(ISkiaCanvas sender)
		{
			EnsureBitmaps();
			if (GameInfo == null)
			{
				return;
			}
		}

		private void DrawBackground(ISkiaCanvas sender, SKSurface args)
		{
			args.Canvas.DrawBitmap(
				_backgroundBitmap,
				new SKRect(0, 0, _backgroundBitmap.Width, _backgroundBitmap.Height),
				new SKRect(0, 0, sender.ScaledSize.Width, sender.ScaledSize.Height));
		}

		private void DrawMirror(ISkiaCanvas sender, SKSurface args)
		{

		}

		private void DrawLaser(ISkiaCanvas sender, SKSurface args)
		{

		}


		private void EnsureBitmaps()
		{
			if (_backgroundBitmap == null)
			{
				var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Game");
				_backgroundBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Background.png"));
				_frontLegBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "FrontLeg.png"));
				_mirrorBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Mirror.png"));
				_planetBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Planet.png"));
			}
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
