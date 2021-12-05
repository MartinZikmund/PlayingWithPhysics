using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.OpticalInstruments.Rendering
{
	public abstract class OpticalInstrumentsRenderer : ISkiaVariantRenderer
	{
		public void Dispose() { }

		public abstract void Draw(ISkiaCanvas sender, SKSurface args);

		public abstract void Update(ISkiaCanvas sender);
	}
}
