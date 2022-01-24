using Physics.FluidFlow.Logic;

namespace Physics.FluidFlow.Rendering
{
	public class ContinuityEquationRenderer : FluidFlowRenderer
	{
		private ContinuityEquationPhysicsService _physicsService;

		public ContinuityEquationRenderer(FluidFlowCanvasController controller) : base(controller)
		{
		}

		internal override void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			_physicsService = new ContinuityEquationPhysicsService(sceneConfiguration);
		}

		public override IPhysicsService PhysicsService => _physicsService;
	}
}
