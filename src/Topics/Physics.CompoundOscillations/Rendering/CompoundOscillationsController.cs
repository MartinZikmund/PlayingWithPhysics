using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.CompoundOscillations.Rendering
{
	public class CompoundOscillationsController : SkiaCanvasController
	{
		public CompoundOscillationsController(SkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}

		public override void Draw(SkiaCanvas sender, SKSurface args) => throw new System.NotImplementedException();
		public override void Update(SkiaCanvas sender) => throw new System.NotImplementedException();
	}
}
