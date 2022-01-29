using Physics.FluidFlow.Logic;

namespace Physics.FluidFlow.Rendering
{
	public class RealFluidMovementRenderer : FluidFlowRenderer
	{
		private BernoulliWithHeightChangePhysicsService _physicsService;

		public RealFluidMovementRenderer(FluidFlowCanvasController controller) : base(controller)
		{
		}

		internal override void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			_physicsService = new BernoulliWithHeightChangePhysicsService(sceneConfiguration);
		}

		public override IPhysicsService PhysicsService => _physicsService;
	}
}
