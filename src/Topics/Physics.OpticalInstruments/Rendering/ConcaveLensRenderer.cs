using Physics.OpticalInstruments.Logic;
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

		protected override float RelativeOpticalInstrumentX => 0.5f;

		protected override InstrumentType InstrumentType => InstrumentType.ConcaveLens;

		protected override bool FlipX => false;

		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawAxisPoint(args, -SceneConfiguration.FocalDistance, "F'");
			DrawAxisPoint(args, SceneConfiguration.FocalDistance, "F");
			DrawLens(sender, args);
		}

		protected override void DrawLens(ISkiaCanvas canvas, SKSurface surface)
		{
			var yBase = GetRenderY(0);
			var yExtentUp = GetRenderY(5);
			var yExtentDown = GetRenderY(-5);
			var x = GetRenderX(0);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentUp), -6, _axisStrokePaint);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentDown), -6, _axisStrokePaint);
		}
	}
}
