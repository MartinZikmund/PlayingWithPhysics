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

		protected override SKPath GetPlumbingStrokePath() => GetPlumbingStroke();

		protected override SKPath GetPlumbingFillPath() => GetPlumbingStroke();

		private SKPath GetPlumbingStroke()
		{
			var diameter = _sceneConfiguration.Diameter1;
			var top = GetRenderY(_physicsService.YMax);
			var bottom = GetRenderY(_physicsService.YMin);
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

		protected override void DrawVectors(SKCanvas canvas)
		{
			// Instead of vectors we draw particle spots
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				//DrawParticleSpot(particleId, 0.7, ) TODO: WTF
			}
		}

		private void DrawParticleSpot(int particleId, float time, SKCanvas canvas)
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
