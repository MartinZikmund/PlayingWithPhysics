using System;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using Windows.UI;

namespace Physics.RotationalInclinedPlane.Rendering
{
	public class SkiaSimulationRenderer : ISkiaVariantRenderer
	{
		private bool _isDisposed = false;
		private readonly RotationalInclinedPlaneCanvasController _canvasController;
		private SimulationBounds _simulationBounds;
		private float _scalingRatio;
		private float _padding = 12;

		private SKPaint _linePaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 2,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		private SKPaint _boxPaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 2,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		private SKPaint _boxStrokePaint = new SKPaint()
		{
			Color = SKColors.Black,
			StrokeWidth = 4,
			IsStroke = true,
			IsAntialias = true,
			FilterQuality = SKFilterQuality.High
		};

		public SkiaSimulationRenderer(RotationalInclinedPlaneCanvasController canvasController)
		{
			_canvasController = canvasController;
		}

		public void Draw(ISkiaCanvas sender, SKSurface args)
		{
			if (_canvasController.PhysicsService == null)
			{
				return;
			}

			if (_scalingRatio < 0 || _isDisposed)
			{
				return;
			}

			var imageInfo = new SKImageInfo((int)(sender.ScaledSize.Width - 2 * _padding), (int)(sender.ScaledSize.Height - _padding), SKImageInfo.PlatformColorType, SKAlphaType.Premul);
			using (var surface = SKSurface.Create(imageInfo))
			{
				var canvas = surface.Canvas;
				canvas.Clear(new SKColor(255, 244, 244, 244));
				DrawFloor(imageInfo, surface);
				DrawInclinedPlane(imageInfo, surface);
				DrawObject(imageInfo, surface);

				args.Canvas.DrawSurface(surface, new SKPoint(_padding, _padding), _linePaint);
			}
		}

		private void DrawObject(SKImageInfo imageInfo, SKSurface surface)
		{
			var t = (float)_canvasController.SimulationTime.TotalTime.TotalSeconds;
			var x = _canvasController.PhysicsService.CalculateX(t) * _scalingRatio;
			var y = _canvasController.PhysicsService.CalculateY(t) * _scalingRatio;

			var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_canvasController.Motion.Color);
			_boxPaint.Color = new SKColor(color.R, color.G, color.B);
			_boxStrokePaint.Color = new SKColor((byte)Math.Max(0, color.R - 60), (byte)Math.Max(0, color.G - 60), (byte)Math.Max(0, color.B - 60));

			var motion = _canvasController.Motion;
			var radiusPx = _scalingRatio * motion.Radius;

			float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
			var centerPoint = new SKPoint(PadX(imageInfo,x) + (float)Math.Cos(rad - Math.PI / 2) * radiusPx, FlipY(imageInfo, y) + (float)Math.Sin(rad - Math.PI / 2) * radiusPx);
			int cx = (int)centerPoint.X;
			int cy = (int)centerPoint.Y;
			if (_canvasController.Motion.BodyType == Logic.BodyType.FullCylinder || _canvasController.Motion.BodyType == Logic.BodyType.Sphere)
			{
				surface.Canvas.DrawCircle(cx, cy, radiusPx, _boxPaint);
			}
			if (_canvasController.Motion.BodyType == Logic.BodyType.FullCylinder || _canvasController.Motion.BodyType == Logic.BodyType.HollowCylinder)
			{
				surface.Canvas.DrawCircle(cx, cy, radiusPx - 2, _boxStrokePaint);
			}

			var totalAngle = _canvasController.PhysicsService.CalculateTotalAngleInRad(t);
			var rotationPoint = new SKPoint(PadX(imageInfo,centerPoint.X) + (float)Math.Cos(totalAngle - Math.PI / 2) * radiusPx, FlipY(imageInfo, centerPoint.Y) + (float)Math.Sin(totalAngle - Math.PI / 2) * radiusPx);
			var point = RotatePointAroundCenter(centerPoint.X, centerPoint.Y, totalAngle, new SKPoint(PadX(imageInfo,x), FlipY(imageInfo, y)));
			surface.Canvas.DrawLine(centerPoint, point, _boxStrokePaint);

			//if (/*t <= _canvasController.PhysicsService.CalculateHorizontalStartTime() ||*/ !_canvasController.Motion.HasHorizontal)
			//{
			//    //var leftTop = new SKPoint(-12, 0);
			//    //var rightBottom = new SKPoint(12, 24);
			//    //var matrix =
			//    //    SKMatrix.Concat(
			//    //    SKMatrix.CreateRotation(MathHelpers.DegreesToRadians(180 + _canvasController.Motion.InclinedAngle)),
			//    //    SKMatrix.CreateTranslation(PadX(x), FlipY(imageInfo, y)));
			//    float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
			//    using (SKPath pathRotated = new SKPath())
			//    {
			//        var centerPoint = new SKPoint(PadX(x) + (float)Math.Cos(rad - Math.PI / 2) * 12, FlipY(imageInfo, y) + (float)Math.Sin(rad - Math.PI / 2) * 12);
			//        int cx = (int)centerPoint.X;
			//        int cy = (int)centerPoint.Y;
			//        pathRotated.MoveTo(SkiaHelpers.RotatePoint(centerPoint - new SKPoint(12,12), cx, cy, rad));
			//        pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint - new SKPoint(12, -12), cx, cy, rad));
			//        pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint + new SKPoint(12, 12), cx, cy, rad));
			//        pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint + new SKPoint(12, -12), cx, cy, rad));
			//        pathRotated.LineTo(SkiaHelpers.RotatePoint(centerPoint - new SKPoint(12, 12), cx, cy, rad));
			//        surface.Canvas.DrawPath(pathRotated, _boxPaint);
			//    }
			//}
			//else
			//{
			//    var left = PadX(x - 12);
			//    var top = FlipY(imageInfo, y + 24);
			//    surface.Canvas.DrawCircle(new SKRect(left, top, left + 24, top + 24), _boxPaint);
			//}
		}

		private SKPoint RotatePointAroundCenter(float cx, float cy, float angle, SKPoint p)
		{
			float s = (float)Math.Sin(angle);
			float c = (float)Math.Cos(angle);

			// translate point back to origin:
			p.X -= cx;
			p.Y -= cy;

			// rotate point
			float xnew = p.X * c - p.Y * s;
			float ynew = p.X * s + p.Y * c;

			// translate point back:
			p.X = xnew + cx;
			p.Y = ynew + cy;
			return p;
		}

		private float PadX(SKImageInfo target, float x)
		{
			var halfWidthInMeters = CalculateTotalWidthInMeters() / 2;
			return ((float)target.Size.Width / 2 - halfWidthInMeters * _scalingRatio + _canvasController.Motion.Radius * _scalingRatio) + x;
		}

		private float FlipY(SKImageInfo target, float y)
		{
			var halfHeightInMeters = CalculateTotalHeightInMeters() / 2;
			return ((float)target.Size.Height / 2 + halfHeightInMeters * _scalingRatio - _canvasController.Motion.Radius * _scalingRatio) - y;
		}

		public void Update(ISkiaCanvas surface)
		{
			if (_canvasController.PhysicsService == null)
			{
				return;
			}

			var reducedSize = surface.ScaledSize.ReduceBy(_padding * 2);
			if (reducedSize.Height < 50 || reducedSize.Width < 50)
			{
				_scalingRatio = -1;
				return;
			}

			var minX = 0;
			var maxX = CalculateTotalWidthInMeters();
			var minY = 0;
			var maxY = CalculateTotalHeightInMeters();

			if (_canvasController.Motion.HorizontalLength == 0)
			{
				_padding = 20;
			}
			else
			{
				_padding = 20;
			}

			_simulationBounds = new SimulationBounds(minX, maxY, maxX, minY);
			_scalingRatio = _simulationBounds.Size.ScaleToFit(surface.ScaledSize.ReduceBy(_padding * 2));
		}

		private float CalculateTotalHeightInMeters()
		{
			var physicsService = _canvasController.PhysicsService;
			return physicsService.CalculateY(0) + 3f * _canvasController.Motion.Radius;
		}

		private float CalculateTotalWidthInMeters()
		{
			var physicsService = _canvasController.PhysicsService;
			return physicsService.CalculateTotalWidth() + 3f * _canvasController.Motion.Radius;
		}

		private float CalculateTopRadiusOffsetInMeters()
		{
			float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
			var centerY = (float)Math.Sin(rad - Math.PI / 2) * _canvasController.Motion.Radius;
			return centerY + _canvasController.Motion.Radius;
		}

		private float CalculateLeftRadiusOffsetInMeters()
		{
			float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
			var centerY = (float)Math.Cos(rad - Math.PI / 2) * _canvasController.Motion.Radius;
			return Math.Abs(centerY - _canvasController.Motion.Radius);
		}

		private float CalculateLeftPaddingInPx()
		{
			return _padding + _canvasController.Motion.Radius * _canvasController.Motion.Radius * _scalingRatio;
		}

		private void DrawInclinedPlane(SKImageInfo info, SKSurface surface)
		{
			var motion = _canvasController.Motion;
			var angleInRad = MathHelpers.DegreesToRadians(motion.InclinedAngle);
			var y = (float)motion.InclinedLength * (float)Math.Sin(angleInRad);
			var x = motion.InclinedLength * Math.Cos(angleInRad);
			surface.Canvas.DrawLine(
				new SKPoint(PadX(info, 0), FlipY(info, (float)y * _scalingRatio)),
				new SKPoint(PadX(info,(float)x * _scalingRatio), FlipY(info, 0)),
				_linePaint);
		}

		private void DrawFloor(SKImageInfo info, SKSurface surface)
		{
			if (_canvasController.Motion.HasHorizontal)
			{
				surface.Canvas.DrawLine(
					new SKPoint(
						PadX(info, _canvasController.PhysicsService.CalculateHorizontalStartX() * _scalingRatio),
						FlipY(info, 0)),
					new SKPoint(
						PadX(info, (float)info.Size.Width - _padding * 2),
						FlipY(info, 0)),
					_linePaint);
			}
		}

		public void Dispose()
		{
			_isDisposed = true;
			_boxPaint?.Dispose();
		}
	}
}
