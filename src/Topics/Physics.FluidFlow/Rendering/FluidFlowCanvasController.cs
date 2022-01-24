using Physics.FluidFlow.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.FluidFlow.Rendering
{
	public class FluidFlowCanvasController : SkiaCanvasController
	{
		public FluidFlowCanvasController(ISkiaCanvas canvasAnimatedControl)
			: base(canvasAnimatedControl)
		{
		}

		public ISkiaCanvas Canvas => _canvas;

		public FluidFlowRenderer Renderer { get; private set; }

		public void SetVariantRenderer(FluidFlowRenderer renderer) => Renderer = renderer;

		public override void Draw(ISkiaCanvas sender, SKSurface args) => Renderer?.Draw(sender, args);

		public override void Update(ISkiaCanvas sender) => Renderer?.Update(sender);

		public void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			Renderer?.StartSimulation(sceneConfiguration);
			SimulationTime.Restart();
			Play();
		}
	}
}
