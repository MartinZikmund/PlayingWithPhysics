using System.Linq;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConvexLensRenderer : LensRenderer
	{
		public ConvexLensRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.5f;

		protected override InstrumentType InstrumentType => InstrumentType.ConvexLens;

		protected override bool FlipX => false;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance, "F");
			DrawAxisPoint(args, SceneConfiguration.FocalDistance, "F'");
			DrawLens(sender, args);
			if (ImageInfo.ImageType != ImageType.None)
			{
				DrawLightBeams(sender, args);
			}
		}

		protected override void DrawLens(ISkiaCanvas canvas, SKSurface surface)
		{
			var yBase = GetRenderY(0);
			var yExtentUp = GetRenderY(5);
			var yExtentDown = GetRenderY(-5);
			var x = GetRenderX(0);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentUp), 6, _axisStrokePaint);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentDown), 6, _axisStrokePaint);
		}

		private void DrawLightBeams(ISkiaCanvas canvas, SKSurface surface)
		{
			// Light beam through mirror center
			var objectTipX = GetRenderX(ObjectPositionX);
			var objectTipY = GetRenderY(SceneConfiguration.ObjectHeight);

			var centerX = GetRenderX(-2 * SceneConfiguration.FocalDistance);
			var centerY = GetRenderY(0);

			var focalX = GetRenderX(-SceneConfiguration.FocalDistance);
			var focalY = GetRenderY(0);

			var imageTipX = GetRenderX(ImageInfo.ImageDistance);
			var imageTipY = GetRenderY(ImageInfo.ImageHeight);

			var zeroX = GetRenderX(0);
			var zeroY = GetRenderY(0);

			var lensIntersectionX = GetRenderX(0);
			surface.Canvas.DrawLine(objectTipX, objectTipY, lensIntersectionX, objectTipY, _lightBeamPaint);

			surface.Canvas.DrawLine(objectTipX, objectTipY, zeroX, zeroY, _imaginaryLightBeamPaint);

			var imageTipLineIntersection = IntersectWithLens(new SKPoint(objectTipX, objectTipY), new SKPoint(imageTipX, imageTipY));

			if (SceneConfiguration.ObjectDistance <= -2 * SceneConfiguration.FocalDistance)
			{
				if (imageTipLineIntersection != null)
				{
					surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y, _lightBeamPaint);
				}
				//surface.Canvas.DrawLine(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, imageTipX, imageTipY, _lightBeamPaint);
			}
			else if (
				SceneConfiguration.ObjectDistance > -2 * SceneConfiguration.FocalDistance &&
				SceneConfiguration.ObjectDistance < -SceneConfiguration.FocalDistance)
			{
				surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipX, imageTipY, _lightBeamPaint);
				//surface.Canvas.DrawLine(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, imageTipX, imageTipY, _lightBeamPaint);
			}
			else
			{
				surface.Canvas.DrawLine(centerX, centerY, imageTipX, imageTipY, _lightBeamPaint);
				surface.Canvas.DrawLine(focalX, focalY, imageTipX, imageTipY, _lightBeamPaint);
			}

			var targetPoint = imageTipLineIntersection.Value;
			var lineToMirror = new Line2d(new Point2d(objectTipX, objectTipY), new Point2d(imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y));
			var lineToImage = new Line2d(new Point2d(objectTipX, objectTipY), new Point2d(imageTipX, imageTipY));
			if (lineToImage.Length > lineToMirror.Length)
			{
				targetPoint = new SKPoint(imageTipX, imageTipY);
			}
		}

		private SKPoint? IntersectWithLens(SKPoint from, SKPoint to)
		{
			var intersections = IntersectionHelpers.FindLineCircleIntersections(GetRenderX(-SceneConfiguration.FocalDistance * 2), GetRenderY(0), 2 * SceneConfiguration.FocalDistance * PixelsPerMeter, new Point2d(from.X, from.Y), new Point2d(to.X, to.Y));
			var intersection = intersections.FirstOrDefault(i => i.X > -SceneConfiguration.FocalDistance * PixelsPerMeter);
			return intersection != null ? new SKPoint((float)intersection.X, (float)intersection.Y) : null;
		}
	}
}
