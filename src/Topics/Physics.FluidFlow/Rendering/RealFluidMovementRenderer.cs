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

		protected override bool ForceXRenderRatio => true;

		protected override float LeftPadding => 8;

		protected override float RightPadding => -8;

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
			var time = GetAdjustedTime();
			// Instead of vectors we draw particle spots
			if (time >= _physicsService.MiddleParticleEndTime * 0.25f)
			{
				DrawParticleSpotsAtTime(_physicsService.MiddleParticleEndTime * 0.25f, canvas);
			}
			if (time >= _physicsService.MiddleParticleEndTime * 0.75)
			{
				DrawParticleSpotsAtTime(_physicsService.MiddleParticleEndTime * 0.75f, canvas);
			}
		}

		private void DrawParticleSpotsAtTime(float time, SKCanvas canvas)
		{
			for (int particleId = 0; particleId < PhysicsService.ParticleCount; particleId++)
			{
				DrawParticleSpot(particleId, time, canvas);
			}
		}

		private void DrawParticleSpot(int particleId, float time, SKCanvas canvas)
		{
			var position = _physicsService.GetParticlePosition(time, particleId);
			canvas.DrawCircle(GetRenderX((float)position.X), GetRenderY((float)position.Y), 4, GetParticlePathPaint(particleId));
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
