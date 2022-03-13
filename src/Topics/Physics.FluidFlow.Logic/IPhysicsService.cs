using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic
{
	public interface IPhysicsService
	{
		bool CanRenderFlow { get; }

		string ErrorKey { get; }

		int ParticleCount { get; }

		float XMax { get; }

		float YMax { get; }

		float YMin { get; }

		float MaxT { get; }

		float Vector1T { get; }

		float Vector2T { get; }

		Point2d GetParticlePosition(float time, int particleId);
	}
}
