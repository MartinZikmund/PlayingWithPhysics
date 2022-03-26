using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic
{
	public interface IPhysicsService
	{
		bool CanRenderFlow { get; }

		string ErrorKey { get; }

		int ParticleCount { get; }

		float SimulationTimeAdjustment { get; }

		float XMax { get; }

		float YMax { get; }

		float YMin { get; }

		float MaxT { get; }

		float Vector1T { get; }

		float Vector2T { get; }

		float V2 { get; }

		float P2 { get; }

		float H1 { get; }

		float H2 { get; }

		Point2d GetParticlePosition(float time, int particleId);
	}
}
