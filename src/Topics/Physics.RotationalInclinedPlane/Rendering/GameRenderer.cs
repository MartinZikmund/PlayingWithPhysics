using System;
using System.IO;
using Physics.RotationalInclinedPlane.Game;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.ApplicationModel;

namespace Physics.RotationalInclinedPlane.Rendering
{
	public class GameRenderer : ISkiaVariantRenderer
	{
		private SKBitmap _backgroundBitmap = null;
		private SKBitmap _objectBitmap = null;

		private readonly SKPaint _rampFillPaint = new SKPaint()
		{
			IsStroke = false,
			Color = SKColors.Gray,
		};

		private readonly SKPaint _rampStrokePaint = new SKPaint()
		{
			IsStroke = true,
			Color = SKColors.Black,
			IsAntialias = true,
			StrokeWidth = 10f
		};

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
			DrawInclinedPlane(sender, args);
			DrawObject(sender, args);
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

		private void DrawObject(ISkiaCanvas sender, SKSurface args)
		{
			
		}

		private void DrawInclinedPlane(ISkiaCanvas sender, SKSurface args)
		{
			var currentAngle = GameInfo.CurrentAngle;

			var baseWidth = sender.ScaledSize.Width * 0.72f;
			var tan = Math.Tan(MathHelpers.DegreesToRadians(currentAngle));
			var height = (float)(baseWidth * tan);

			SKPath path = new SKPath();
			var topLeft = new SKPoint(-sender.ScaledSize.Width * 0.02f, sender.ScaledSize.Height * 0.9f - height);
			var bottomLeft = new SKPoint(-sender.ScaledSize.Width * 0.02f, sender.ScaledSize.Height * 0.9f);
			var bottomRight = new SKPoint(sender.ScaledSize.Width * 0.7f, bottomLeft.Y);

			path.MoveTo(topLeft);
			path.LineTo(bottomRight);
			path.LineTo(bottomLeft);
			path.LineTo(topLeft);
			path.Close();

			args.Canvas.DrawPath(path, _rampFillPaint);
			args.Canvas.DrawPath(path, _rampStrokePaint);
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
