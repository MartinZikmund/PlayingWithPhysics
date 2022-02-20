using Physics.FluidFlow.Logic;
using SkiaSharp;

namespace Physics.FluidFlow.Rendering
{
	public class RealFluidMovementRenderer : FluidFlowRenderer
	{
		private RealFluidFlowPhysicsService _physicsService;
		private SceneConfiguration _sceneConfiguration;

		public RealFluidMovementRenderer(FluidFlowCanvasController controller) : base(controller)
		{
		}

		internal override void StartSimulation(SceneConfiguration sceneConfiguration)
		{
			base.StartSimulation(sceneConfiguration);
			_sceneConfiguration = sceneConfiguration;
			_physicsService = new RealFluidFlowPhysicsService(sceneConfiguration);
		}

		public override IPhysicsService PhysicsService => _physicsService;

		protected override float GetVelocityVectorSize(int vectorId, int particleId) => 0;
	}
}
