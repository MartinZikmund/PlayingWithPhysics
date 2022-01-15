using System;
using System.Linq;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConcaveMirrorRenderer : MirrorRenderer
	{
		public ConcaveMirrorRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.6f;

		protected override InstrumentType InstrumentType => InstrumentType.ConcaveMirror;

		protected override bool FlipX => true;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, SceneConfiguration.FocalDistance, "F");
			DrawAxisPoint(args, SceneConfiguration.FocalDistance * 2, "C");
			DrawMirror(sender, args);
			if (ImageInfo.ImageType != ImageType.None && !SceneConfiguration.IsLoading)
			{
				DrawLightBeams(sender, args);
			}
		}

		protected override void DrawMirror(ISkiaCanvas canvas, SKSurface surface)
		{
			var centerX = GetRenderX(SceneConfiguration.FocalDistance * 2);
			var centerY = GetRenderY(0);
			var radius = MirrorRadius * PixelsPerMeter;
			var bounds = new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius);
			var shadowBounds = new SKRect(centerX + _mirrorShadowPaint.StrokeWidth / 2 - radius, centerY - radius, centerX + _mirrorShadowPaint.StrokeWidth / 2 + radius, centerY + radius);

			using var path = new SKPath();
			path.AddArc(bounds, -40, 80);

			using var shadowPath = new SKPath();
			shadowPath.AddArc(shadowBounds, -41, 82);

			surface.Canvas.DrawPath(shadowPath, _mirrorShadowPaint);
			surface.Canvas.DrawPath(path, _axisStrokePaint);
		}

		private float GetIntersectionLimitX()
		{
			var radius = Math.Abs(SceneConfiguration.FocalDistance) * 2;
			return (float)(radius - Math.Cos(MathHelpers.DegreesToRadians(40)) * radius);
		}

		private void DrawLightBeams(ISkiaCanvas canvas, SKSurface surface)
		{
			// Light beam through mirror center
			var objectTipX = GetRenderX(ObjectPositionX);
			var objectTipY = GetRenderY(SceneConfiguration.ObjectHeight);

			var centerX = GetRenderX(2 * SceneConfiguration.FocalDistance);
			var centerY = GetRenderY(0);

			var focalX = GetRenderX(SceneConfiguration.FocalDistance);
			var focalY = GetRenderY(0);

			var imageTipX = GetRenderX(ImageInfo.ImageDistance);
			var imageTipY = GetRenderY(ImageInfo.ImageHeight);

			var zeroX = GetRenderX(0);

			var parallelLineIntersection = IntersectWithMirror(new SKPoint(objectTipY, objectTipY), new SKPoint(GetRenderX(0), objectTipY));
			var imageTipLineIntersection = Math.Abs(objectTipX - imageTipX) > 1 ? IntersectWithMirror(new SKPoint(objectTipX, objectTipY), new SKPoint(imageTipX, imageTipY)) : null;

			Point2d? axisIntersection = null;

			if (parallelLineIntersection != null)
			{
				var lineParallelToImage = new Line2d(new Point2d(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y), new Point2d(imageTipX, imageTipY));
				var lineAxis = new Line2d(new Point2d(GetRenderX(0), GetRenderY(0)), new Point2d(focalX, focalY));
				axisIntersection = lineParallelToImage.IntersectWith(lineAxis);

				if (axisIntersection != null && !IsValidFocalLightBeamDistance(GetRealX((float)axisIntersection.Value.X)))
				{
					axisIntersection = null;
				}
			}

			if (SceneConfiguration.ObjectDistance >= 2 * SceneConfiguration.FocalDistance)
			{
				if (axisIntersection != null)
				{
					surface.Canvas.DrawLine(objectTipX, objectTipY, parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, _lightBeamPaint);

					var limitXReal = GetIntersectionLimitX();
					var limitX = GetRenderX(limitXReal);
					if (imageTipLineIntersection != null && imageTipLineIntersection.Value.X >= limitX)
					{
						surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y, _lightBeamPaint);
					}
					else
					{
						surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipX, imageTipY, _lightBeamPaint);
					}
					surface.Canvas.DrawLine(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, imageTipX, imageTipY, _lightBeamPaint);
				}
			}
			else if (
				SceneConfiguration.ObjectDistance <= 2 * SceneConfiguration.FocalDistance &&
				SceneConfiguration.ObjectDistance > SceneConfiguration.FocalDistance)
			{
				surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipX, imageTipY, _lightBeamPaint);
				if (axisIntersection != null)
				{
					surface.Canvas.DrawLine(objectTipX, objectTipY, parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, _lightBeamPaint);

					surface.Canvas.DrawLine(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, imageTipX, imageTipY, _lightBeamPaint);
				}
			}
			else
			{
				var focalLineIntersection = IntersectWithMirror(new SKPoint(focalX, focalY), new SKPoint(imageTipX, imageTipY));
				if (focalLineIntersection != null)
				{
					surface.Canvas.DrawLine(focalX, focalY, focalLineIntersection.Value.X, focalLineIntersection.Value.Y, _lightBeamPaint);
					surface.Canvas.DrawLine(focalLineIntersection.Value.X, focalLineIntersection.Value.Y, imageTipX, imageTipY, _imaginaryLightBeamPaint);
				}

				if (imageTipLineIntersection != null)
				{
					surface.Canvas.DrawLine(centerX, centerY, imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y, _lightBeamPaint);
					surface.Canvas.DrawLine(imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y, imageTipX, imageTipY, _imaginaryLightBeamPaint);
				}

				if (axisIntersection != null)
				{
					surface.Canvas.DrawLine(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, zeroX, objectTipY, _imaginaryLightBeamPaint);
				}
			}
		}

		private SKPoint? IntersectWithMirror(SKPoint from, SKPoint to)
		{
			var intersections = IntersectionHelpers.FindLineCircleIntersections(GetRenderX(SceneConfiguration.FocalDistance * 2), GetRenderY(0), MirrorRadius * PixelsPerMeter, new Point2d(from.X, from.Y), new Point2d(to.X, to.Y));
			var intersection = intersections.FirstOrDefault(i => i.X > GetRenderX(SceneConfiguration.FocalDistance * 2));
			return intersection != null ? new SKPoint((float)intersection.X, (float)intersection.Y) : null;
		}
	}
}
