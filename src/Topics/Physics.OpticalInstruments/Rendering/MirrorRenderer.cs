using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public abstract class MirrorRenderer : OpticalInstrumentsRenderer
	{
		protected MirrorRenderer(OpticalInstrumentsCanvasController controller) : base(controller)
		{
		}

		protected float MirrorRadius => SceneConfiguration.FocalDistance * 2;

		protected abstract void DrawMirror(ISkiaCanvas canvas, SKSurface surface);
	}
}
