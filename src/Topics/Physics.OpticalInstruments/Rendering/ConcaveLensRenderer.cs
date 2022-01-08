using Physics.OpticalInstruments.Logic;
using Physics.Shared.Logic.Geometry;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public class ConcaveLensRenderer : LensRenderer
	{
		public ConcaveLensRenderer(OpticalInstrumentsCanvasController controller) :
			base(controller)
		{
		}

		protected override float RelativeOpticalInstrumentX => 0.75f;

		protected override InstrumentType InstrumentType => InstrumentType.ConcaveLens;

		protected override bool FlipX => false;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance, "F");
			DrawAxisPoint(args, SceneConfiguration.FocalDistance, "F'");
			DrawLens(sender, args);
			if (ImageInfo.ImageType != ImageType.None && !SceneConfiguration.IsLoading)
			{
				DrawLightBeams(sender, args);
			}
		}

		protected override void DrawLens(ISkiaCanvas canvas, SKSurface surface)
		{
			var yBase = GetRenderY(0);
			var yExtentUp = GetRenderY(SceneConfiguration.FocalDistance * 1.3f);
			var yExtentDown = GetRenderY(-SceneConfiguration.FocalDistance * 1.3f);
			var x = GetRenderX(0);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentUp), -6, _axisStrokePaint);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentDown), -6, _axisStrokePaint);
		}

		private void DrawLightBeams(ISkiaCanvas canvas, SKSurface surface)
		{
			// Light beam through mirror center
			var objectTipX = GetRenderX(ObjectPositionX);
			var objectTipY = GetRenderY(SceneConfiguration.ObjectHeight);

			var centerX = GetRenderX(-2 * SceneConfiguration.FocalDistance);
			var centerY = GetRenderY(0);

			var focalX = GetRenderX(SceneConfiguration.FocalDistance);
			var focalY = GetRenderY(0);

			var imageTipX = GetRenderX(ImageInfo.ImageDistance);
			var imageTipY = GetRenderY(ImageInfo.ImageHeight);

			var zeroX = GetRenderX(0);
			var zeroY = GetRenderY(0);

			var lensIntersectionX = GetRenderX(0);
			surface.Canvas.DrawLine(objectTipX, objectTipY, lensIntersectionX, objectTipY, _lightBeamPaint);

			// Center to image lines
			surface.Canvas.DrawLine(objectTipX, objectTipY, zeroX, zeroY, _lightBeamPaint);
			surface.Canvas.DrawLine(objectTipX, objectTipY, imageTipX, imageTipY, _imaginaryLightBeamPaint);

			// Focal to image lines
			surface.Canvas.DrawLine(zeroX, objectTipY, focalX, zeroY, _imaginaryLightBeamPaint);
		}
	}
}
