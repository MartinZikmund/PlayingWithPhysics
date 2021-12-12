using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public abstract class LensRenderer : OpticalInstrumentsRenderer
	{
		protected LensRenderer(OpticalInstrumentsCanvasController controller) : base(controller)
		{
		}

		protected abstract void DrawLens(ISkiaCanvas canvas, SKSurface surface);
	}
}
