using System;

namespace Physics.FluidFlow.Logic
{
	public class ParticleTrajectoryPoint
	{
		public ParticleTrajectoryPoint(TimeSpan time, float x, float y)
		{
			Time = time;
			X = x;
			Y = y;
		}

		public TimeSpan Time { get; }

		public float X { get; }

		public float Y { get; }
	}
}
