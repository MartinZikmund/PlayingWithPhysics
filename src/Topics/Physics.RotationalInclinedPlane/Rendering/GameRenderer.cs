using System.IO;
using Physics.RotationalInclinedPlane.Game;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.RotationalInclinedPlane.Rendering
{
	public class GameRenderer : ISkiaVariantRenderer
	{
		private SKBitmap _backgroundBitmap = null;
		private SKBitmap _objectBitmap = null;

		public GameRenderer(RotationalInclinedPlaneCanvasController controller)
		{
		}

		public GameInfo GameInfo { get; internal set; }

		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (GameInfo is null)
			{
				return;
			}
			
			DrawBackground(sender, args);
		}

		public void Update(ISkiaCanvas sender)
		{
			EnsureBitmaps();
			if (GameInfo is null)
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


		private void EnsureBitmaps()
		{
			if (_backgroundBitmap == null)
			{
				var gameAssetsPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Game");
				_backgroundBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "BackgroundTemp.jpg"));
				_objectBitmap = SKBitmap.Decode(Path.Combine(gameAssetsPath, "Object.png"));
			}
		}
	}
}
