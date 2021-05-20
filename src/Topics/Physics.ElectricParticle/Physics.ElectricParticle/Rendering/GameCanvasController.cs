using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.ElectricParticle.Rendering
{
	public class GameCanvasController : SkiaCanvasController
	{
		public GameCanvasController(ISkiaCanvas control):base(control)
		{

		}

		public override void Draw(ISkiaCanvas sender, SKSurface args) => throw new System.NotImplementedException();
		public override void Update(ISkiaCanvas sender) => throw new System.NotImplementedException();
	}
}
