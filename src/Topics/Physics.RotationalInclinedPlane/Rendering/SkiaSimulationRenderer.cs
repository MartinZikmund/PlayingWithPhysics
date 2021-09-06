using System;
using System.Linq;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RotationalInclinedPlane.Rendering
{
	public class SkiaSimulationRenderer : ISkiaVariantRenderer
	{
		private bool _isDisposed = false;
		private readonly RotationalInclinedPlaneCanvasController _canvasController;
		private SimulationBounds _simulationBounds;
		private float _scalingRatio;
		private float _padding = 12;

		private float _maxRadius = 0.0f;

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
			StrokeWidth = 2,
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
			if (_canvasController.PhysicsServices.Length == 0)
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
				DrawInclinedPlanes(imageInfo, surface);
				DrawObjects(imageInfo, surface);

				args.Canvas.DrawSurface(surface, new SKPoint(_padding, _padding), _linePaint);
			}
		}

		private void DrawObjects(SKImageInfo imageInfo, SKSurface surface)
		{
			var maxX = _canvasController.PhysicsServices.Max(s => s.Setup.InclinedLength * Math.Cos(MathHelpers.DegreesToRadians(s.Setup.InclinedAngle)));

			foreach (var physicsService in _canvasController.PhysicsServices)
			{
				var thisHorizontalWidth = physicsService.CalculateInclinedWidth();
				var startOffset = maxX - thisHorizontalWidth;

				var t = (float)_canvasController.SimulationTime.TotalTime.TotalSeconds;
				var x = (float)(startOffset + physicsService.CalculateX(t)) * _scalingRatio;
				var y = physicsService.CalculateY(t) * _scalingRatio;

				var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(physicsService.Setup.Color);
				_boxPaint.Color = new SKColor(color.R, color.G, color.B, 180);
				_boxStrokePaint.Color = new SKColor((byte)Math.Max(0, color.R - 60), (byte)Math.Max(0, color.G - 60), (byte)Math.Max(0, color.B - 60));

				var motion = physicsService.Setup;
				var radiusPx = _scalingRatio * motion.Radius;

				float rad = MathHelpers.DegreesToRadians(physicsService.Setup.InclinedAngle);
				var centerPoint = new SKPoint(PadX(imageInfo, x) + (float)Math.Cos(rad - Math.PI / 2) * radiusPx, FlipY(imageInfo, y) + (float)Math.Sin(rad - Math.PI / 2) * radiusPx);
				int cx = (int)centerPoint.X;
				int cy = (int)centerPoint.Y;
				if (motion.BodyType == Logic.BodyType.FullCylinder || motion.BodyType == Logic.BodyType.Sphere)
				{
					surface.Canvas.DrawCircle(cx, cy, radiusPx, _boxPaint);
				}
				if (motion.BodyType == Logic.BodyType.FullCylinder || motion.BodyType == Logic.BodyType.HollowCylinder)
				{
					surface.Canvas.DrawCircle(cx, cy, radiusPx - 1, _boxStrokePaint);
				}

				var totalAngle = physicsService.CalculateTotalAngleInRad(t);
				var rotationPoint = new SKPoint(PadX(imageInfo, centerPoint.X) + (float)Math.Cos(totalAngle - Math.PI / 2) * radiusPx, FlipY(imageInfo, centerPoint.Y) + (float)Math.Sin(totalAngle - Math.PI / 2) * radiusPx);
				var point = RotatePointAroundCenter(centerPoint.X, centerPoint.Y, totalAngle, new SKPoint(PadX(imageInfo, x), FlipY(imageInfo, y)));
				surface.Canvas.DrawLine(centerPoint, point, _boxStrokePaint);
			}
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
			return ((float)target.Size.Width / 2 - halfWidthInMeters * _scalingRatio + _maxRadius * _scalingRatio) + x;
		}

		private float FlipY(SKImageInfo target, float y)
		{
			var halfHeightInMeters = CalculateTotalHeightInMeters() / 2;
			return ((float)target.Size.Height / 2 + halfHeightInMeters * _scalingRatio - _maxRadius * _scalingRatio) - y;
		}

		public void Update(ISkiaCanvas surface)
		{
			if (_canvasController.PhysicsServices.Length == 0)
			{
				return;
			}

			_maxRadius = GetMaxRadius();

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

			_padding = 20;

			_simulationBounds = new SimulationBounds(minX, maxY, maxX, minY);
			_scalingRatio = _simulationBounds.Size.ScaleToFit(surface.ScaledSize.ReduceBy(_padding * 2));
		}

		private float CalculateTotalHeightInMeters() => _canvasController.PhysicsServices.Max(s => s.CalculateY(0) + 3f * s.Setup.Radius);

		private float CalculateTotalWidthInMeters() => _canvasController.PhysicsServices.Max(s => s.CalculateTotalWidth()) + 3f * _maxRadius;

		private float GetMaxRadius() => _canvasController.Motions.Max(m => m.Radius);

		//private float CalculateTopRadiusOffsetInMeters()
		//{
		//	float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
		//	var centerY = (float)Math.Sin(rad - Math.PI / 2) * _canvasController.Motion.Radius;
		//	return centerY + _canvasController.Motion.Radius;
		//}

		//private float CalculateLeftRadiusOffsetInMeters()
		//{
		//	float rad = MathHelpers.DegreesToRadians(_canvasController.Motion.InclinedAngle);
		//	var centerY = (float)Math.Cos(rad - Math.PI / 2) * _canvasController.Motion.Radius;
		//	return Math.Abs(centerY - _canvasController.Motion.Radius);
		//}

		//private float CalculateLeftPaddingInPx()
		//{
		//	return _padding + _canvasController.Motions[0].Radius * _canvasController.Motion.Radius * _scalingRatio;
		//}

		private void DrawInclinedPlanes(SKImageInfo info, SKSurface surface)
		{
			var maxX = _canvasController.PhysicsServices.Max(s => s.Setup.InclinedLength * Math.Cos(MathHelpers.DegreesToRadians(s.Setup.InclinedAngle)));

			foreach (var service in _canvasController.PhysicsServices)
			{
				var motion = service.Setup;
				var angleInRad = MathHelpers.DegreesToRadians(motion.InclinedAngle);
				var y = (float)motion.InclinedLength * (float)Math.Sin(angleInRad);
				var x = motion.InclinedLength * Math.Cos(angleInRad);

				var from = new SKPoint(PadX(info, (float)(maxX - x) * _scalingRatio), FlipY(info, (float)y * _scalingRatio));
				var to = new SKPoint(PadX(info, (float)maxX * _scalingRatio), FlipY(info, 0));
				surface.Canvas.DrawLine(from, to, _linePaint);
			}
		}

		private void DrawFloor(SKImageInfo info, SKSurface surface)
		{
			if (_canvasController.Motions[0].HasHorizontal)
			{
				surface.Canvas.DrawLine(
					new SKPoint(
						PadX(info, _canvasController.PhysicsServices[0].CalculateHorizontalStartX() * _scalingRatio),
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
