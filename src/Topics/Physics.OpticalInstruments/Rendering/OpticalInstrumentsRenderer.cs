using System;
using Microsoft.AppCenter.Crashes;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public abstract class OpticalInstrumentsRenderer : ISkiaVariantRenderer
	{
		private const float AxisPointSize = 4;

		protected readonly OpticalInstrumentsCanvasController _controller;

		private SKSize _canvasSize;

		protected readonly SKPaint _axisStrokePaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 2,
		};

		protected readonly SKPaint _axisLabelPaint = new SKPaint()
		{
			Color = SKColors.Black,
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 1,
			TextAlign = SKTextAlign.Center,
			TextSize = 12,
		};

		protected readonly SKPaint _objectPaint = new SKPaint()
		{
			Color = SKColors.Red,
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 2
		};

		protected readonly SKPaint _objectImagePaint = new SKPaint()
		{
			Color = SKColors.HotPink,
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 2
		};

		protected readonly SKPaint _lightBeamPaint = new SKPaint()
		{
			Color = SKColors.Blue,
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 2
		};

		internal bool TryGetObjectPosition(SKPoint pointerPoint, out SKPoint objectPoint)
		{
			var metersPerPixel = 1 / PixelsPerMeter;
			var centerX = GetRenderX(0);
			var centerY = GetRenderY(0);

			objectPoint = SKPoint.Empty;
			if (pointerPoint.X >= centerX)
			{
				return false;
			}

			var distance = pointerPoint.X - centerX;
			var height = centerY - pointerPoint.Y;
			objectPoint = new SKPoint(distance * metersPerPixel, height * metersPerPixel);
			return true;
		}

		public OpticalInstrumentsRenderer(OpticalInstrumentsCanvasController controller) =>
			_controller = controller;

		protected SceneConfiguration SceneConfiguration => _controller.SceneConfiguration;

		protected PhysicsService PhysicsService { get; } = new PhysicsService();

		protected float PixelsPerMeter { get; private set; }

		protected abstract float RelativeOpticalInstrumentX { get; }

		protected abstract InstrumentType InstrumentType { get; }

		protected float MinX { get; private set; }

		protected float MaxX { get; private set; }

		public void Dispose() { }

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			DrawXAxis(sender, args);
			DrawConfiguration(sender, args);
			DrawObject(args);
			DrawObjectImage(args);
		}

		public void Update(ISkiaCanvas sender)
		{
			_canvasSize = sender.ScaledSize;
			UpdateScalingRatio();
			UpdateBounds();

			ImageInfo = PhysicsService.CalculateObjectImage(InstrumentType, SceneConfiguration);
		}

		protected virtual void DrawConfiguration(ISkiaCanvas sender, SKSurface args) { }

		protected void DrawAxisPoint(SKSurface args, float x, string label)
		{
			var renderX = GetRenderX(x);
			var y = GetRenderY(0);
			args.Canvas.DrawLine(renderX, y - AxisPointSize, renderX, y + AxisPointSize, _axisStrokePaint);
			args.Canvas.DrawText(label, renderX, y + AxisPointSize + 14, _axisLabelPaint);
		}

		protected float GetRenderX(float xInMeters) =>
			_canvasSize.Width * RelativeOpticalInstrumentX + xInMeters * PixelsPerMeter;

		protected float GetRenderY(float yInMeters) =>
			_canvasSize.Height / 2 - yInMeters * PixelsPerMeter;

		protected void DrawObject(SKSurface args)
		{
			var renderX = GetRenderX(SceneConfiguration.ObjectDistance);
			var from = new SKPoint(renderX, GetRenderY(0));
			var to = new SKPoint(renderX, GetRenderY(SceneConfiguration.ObjectHeight));
			ArrowRenderer.Draw(args, from, to, 6, _objectPaint);
		}

		protected void DrawObjectImage(SKSurface args)
		{
			var renderX = GetRenderX(ImageInfo.ImageDistance);
			var from = new SKPoint(renderX, GetRenderY(0));
			var to = new SKPoint(renderX, GetRenderY(ImageInfo.ImageHeight));
			ArrowRenderer.Draw(args, from, to, 6, _objectImagePaint);
		}

		protected ObjectImageInfo ImageInfo { get; private set; }

		private void DrawXAxis(ISkiaCanvas sender, SKSurface args)
		{
			var y = GetRenderY(0);
			args.Canvas.DrawLine(0, y, sender.ScaledSize.Width, y, _axisStrokePaint);
		}

		private void UpdateScalingRatio()
		{
			var focalDistance = _controller.SceneConfiguration.FocalDistance;

			// Screen should be focal distance * 5 wide
			var widthInMeters = focalDistance * 5;
			var pixelsPerMeterX = _canvasSize.Width / widthInMeters;

			// Screen should be focal distance * 4 high
			var heightInMeters = focalDistance * 4;
			var pixelsPerMeterY = _canvasSize.Height / heightInMeters;

			// Take the lower value
			PixelsPerMeter = Math.Min(pixelsPerMeterX, pixelsPerMeterY);
		}

		private void UpdateBounds()
		{
			// X = 0 is at the optical instrument's center.
			var opticalInstrumentRenderX = _canvasSize.Width * RelativeOpticalInstrumentX;
			MinX = -(opticalInstrumentRenderX / PixelsPerMeter);
			MaxX = (_canvasSize.Width - opticalInstrumentRenderX) / PixelsPerMeter;
		}
	}
}
