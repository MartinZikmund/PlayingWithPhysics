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

		protected override SKPath GetPlumbingPath()
		{
			var diameter = _sceneConfiguration.Diameter1;
			var top = GetRenderY(-diameter / 2);
			var bottom = GetRenderY(diameter / 2);
			var left = -10;
			var right = _canvas.ScaledSize.Width + 10;

			var path = new SKPath();
			path.MoveTo(left, top);
			path.LineTo(right, top);
			path.LineTo(right, bottom);
			path.LineTo(left, bottom);
			path.Close();
			return path;
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
