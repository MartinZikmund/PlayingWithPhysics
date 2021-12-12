using Physics.OpticalInstruments.Logic;
﻿using Physics.Shared.UI.Rendering.Skia;
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
		
		protected override void DrawConfiguration(ISkiaCanvas sender, SKSurface args)
		{
			DrawLens(sender, args);
		}

		protected override void DrawLens(ISkiaCanvas canvas, SKSurface surface)
		{
			var yBase = GetRenderY(0);
			var yExtentUp = GetRenderY(3);
			var yExtentDown = GetRenderY(-3);
			var x = GetRenderX(RelativeOpticalInstrumentX);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentUp), 6, _axisStrokePaint);
			ArrowRenderer.Draw(surface, new SKPoint(x, yBase), new SKPoint(x, yExtentDown), 6, _axisStrokePaint);
		}
	}
}
