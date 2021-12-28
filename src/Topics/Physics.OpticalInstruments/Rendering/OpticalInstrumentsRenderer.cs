using System;
using Microsoft.AppCenter.Crashes;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.Helpers;
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

		protected readonly SKPaint _imaginaryLightBeamPaint = new SKPaint()
		{
			Color = SKColors.Blue,
			IsStroke = true,
			IsAntialias = true,
			StrokeWidth = 2,
			PathEffect = SKPathEffect.CreateDash(new[] { 4f, 2f }, 0)
		};

		internal bool TryGetObjectPosition(SKPoint pointerPoint, out SKPoint objectPoint)
		{
			var metersPerPixel = 1 / PixelsPerMeter;
			var centerX = GetRenderX(0);
			var centerY = GetRenderY(0);

			objectPoint = SKPoint.Empty;
			if (pointerPoint.X >= centerX)
			{
				pointerPoint.X = centerX;
			}

			var distance = pointerPoint.X - centerX;
			var actualDistance = Math.Abs(distance * metersPerPixel);
			if (actualDistance < Math.Abs(SceneConfiguration.FocalDistance) / 10)
			{
				actualDistance = Math.Abs(SceneConfiguration.FocalDistance) / 10;
			}
			var height = centerY - pointerPoint.Y;
			var actualHeight = height * metersPerPixel;
			var heightClampScale =
				InstrumentType == InstrumentType.ConcaveLens ||
				InstrumentType == InstrumentType.ConvexLens ?
					1.5f : 0.8f;
			actualHeight = MathHelpers.Clamp(
				actualHeight,
				-Math.Abs(SceneConfiguration.FocalDistance * heightClampScale),
				Math.Abs(SceneConfiguration.FocalDistance * heightClampScale));
			objectPoint = new SKPoint(actualDistance, actualHeight);
			return true;
		}

		public OpticalInstrumentsRenderer(OpticalInstrumentsCanvasController controller) =>
			_controller = controller;

		protected SceneConfiguration SceneConfiguration => _controller.SceneConfiguration;

		protected float ObjectPositionX => FlipX ? SceneConfiguration.ObjectDistance : -SceneConfiguration.ObjectDistance;

		protected PhysicsService PhysicsService { get; } = new PhysicsService();

		protected float PixelsPerMeter { get; private set; }

		protected abstract float RelativeOpticalInstrumentX { get; }

		protected abstract bool FlipX { get; }

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
			_canvasSize.Width * RelativeOpticalInstrumentX + (xInMeters * (FlipX ? -1 : 1)) * PixelsPerMeter;

		protected float GetRenderY(float yInMeters) =>
			_canvasSize.Height / 2 - yInMeters * PixelsPerMeter;

		protected void DrawObject(SKSurface args)
		{
			var renderX = GetRenderX(ObjectPositionX);
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

			// Screen should be focal distance * 7 wide
			var widthInMeters = Math.Abs(focalDistance) * 7;
			var pixelsPerMeterX = _canvasSize.Width / widthInMeters;

			// Screen should be focal distance * ?? high
			var heightInMeters = 0f;
			if (InstrumentType == InstrumentType.ConcaveLens || InstrumentType == InstrumentType.ConvexLens)
			{
				heightInMeters = Math.Abs(focalDistance) * 5f;
			}
			else
			{
				heightInMeters = Math.Abs(focalDistance) * 3f;
			}
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
