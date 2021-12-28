using System.Linq;
using Physics.OpticalInstruments.Logic;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConvexMirrorRenderer : MirrorRenderer
	{
		public ConvexMirrorRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.5f;

		protected override bool FlipX => true;

		protected override InstrumentType InstrumentType => InstrumentType.ConvexMirror;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, SceneConfiguration.FocalDistance, "F");
			DrawAxisPoint(args, SceneConfiguration.FocalDistance * 2, "C");
			DrawMirror(sender, args);
			if (ImageInfo.ImageType != ImageType.None)
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

			using var path = new SKPath();
			path.AddArc(bounds, -150, -60);
			surface.Canvas.DrawPath(path, _axisStrokePaint);
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

			var parallelLineIntersection = IntersectWithMirror(new SKPoint(objectTipX, objectTipY), new SKPoint(GetRenderX(0), objectTipY));
			if (parallelLineIntersection != null)
			{
				surface.Canvas.DrawLine(objectTipX, objectTipY, parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, _lightBeamPaint);

				var lineMirror = new Line2d(new Point2d(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y), new Point2d(imageTipX, imageTipY));
				var lineAxis = new Line2d(new Point2d(GetRenderX(0), GetRenderY(0)), new Point2d(focalX, focalY));
				var intersection = lineMirror.IntersectWith(lineAxis);
				if (intersection != null)
				{
					surface.Canvas.DrawLine(parallelLineIntersection.Value.X, parallelLineIntersection.Value.Y, (float)intersection.Value.X, (float)intersection.Value.Y, _imaginaryLightBeamPaint);
				}
			}

			var imageTipLineIntersection = IntersectWithMirror(new SKPoint(objectTipX, objectTipY), new SKPoint(imageTipX, imageTipY));
			if (imageTipLineIntersection != null)
			{
				surface.Canvas.DrawLine(imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y, centerX, centerY, _imaginaryLightBeamPaint);
				surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipLineIntersection.Value.X, imageTipLineIntersection.Value.Y, _lightBeamPaint);
			}

		}

		private SKPoint? IntersectWithMirror(SKPoint from, SKPoint to)
		{
			var intersections = IntersectionHelpers.FindLineCircleIntersections(GetRenderX(SceneConfiguration.FocalDistance * 2), GetRenderY(0), MirrorRadius * PixelsPerMeter, new Point2d(from.X, from.Y), new Point2d(to.X, to.Y));
			var intersection = intersections.FirstOrDefault(i => i.X < GetRenderX(SceneConfiguration.FocalDistance * 2));
			return intersection != null ? new SKPoint((float)intersection.X, (float)intersection.Y) : null;
		}
	}
}
