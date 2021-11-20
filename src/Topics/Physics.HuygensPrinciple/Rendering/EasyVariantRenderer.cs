using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.HuygensPrinciple.Rendering
{
	public class EasyVariantRenderer : IHuygensVariantRenderer
	{
		private readonly HuygensPrincipleCanvasController _controller;

		public EasyVariantRenderer(HuygensPrincipleCanvasController controller)
		{
			_controller = controller;
		}

		public void Dispose() => throw new System.NotImplementedException();
		public void Draw(ISkiaCanvas sender, SKSurface args) => throw new System.NotImplementedException();
		public void StartSimulation() => throw new System.NotImplementedException();
		public void Update(ISkiaCanvas sender) => throw new System.NotImplementedException();
	}
}
