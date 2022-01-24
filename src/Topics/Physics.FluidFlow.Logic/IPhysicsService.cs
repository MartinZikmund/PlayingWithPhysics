﻿using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic
{
	public interface IPhysicsService
	{
		int ParticleCount { get; }

		float XMax { get; }

		float YMax { get; }

		Point2d GetParticlePosition(float time, int particleId);
	}
}
