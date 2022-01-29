using System;
using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic
{
	public class BernoulliWithHeightChangePhysicsService : IPhysicsService
	{
		private readonly SceneConfiguration _input;

		public BernoulliWithHeightChangePhysicsService(SceneConfiguration input)
		{
			_input = input;
		}

		public int ParticleCount => _input.DiameterRelationType == DiameterRelationType.Equal ? 7 : 5;

		public float XMax =>
			_input.DiameterRelationType switch
			{
				DiameterRelationType.Equal => 58 * _input.Velocity,
				DiameterRelationType.S1Larger => 100,
				DiameterRelationType.S2Larger => 100,
				_ => throw new InvalidOperationException("Invalid diameter type"),
			};

		public float YMax =>
			_input.DiameterRelationType switch
			{
				DiameterRelationType.S1Larger => 60,
				DiameterRelationType.S2Larger => 60,
				_ => throw new InvalidOperationException("Invalid diameter type"),
			};

		public float YMin => 0;

		public Point2d GetParticlePosition(float time, int particleId) =>
			_input.DiameterRelationType switch
			{
				DiameterRelationType.S1Larger => GetS1LargerParticlePosition(time, particleId),
				DiameterRelationType.S2Larger => GetS2LargerParticlePosition(time, particleId),
				_ => throw new InvalidOperationException("Invalid diameter type"),
			};

		private Point2d GetS1LargerParticlePosition(float time, int particleId)
		{
			if (time < 40)
			{
				return particleId switch
				{
					0 => new Point2d(time, 60),
					1 => new Point2d(time, 50),
					2 => new Point2d(time, 40),
					3 => new Point2d(time, 30),
					4 => new Point2d(time, 20),
					_ => throw new InvalidOperationException()
				};
			}
			else if (time < 48)
			{
				var x = 40 + (time - 40) + 3 / 16.0 * (time - 40) * (time - 40);
				var xDiff = x - 40;
				return particleId switch
				{
					0 => new Point2d(x, 60 - xDiff * 2),
					1 => new Point2d(x, 50 - xDiff * 7/4.0),
					2 => new Point2d(x, 40 - xDiff * 3/2.0),
					3 => new Point2d(x, 30 - xDiff * 1.25),
					4 => new Point2d(x, 20 - xDiff),
					_ => throw new InvalidOperationException()
				};
			}
			else
			{
				time = Math.Min(time, 58);
				var x = 60 + 4 * (time - 48);
				return particleId switch
				{
					0 => new Point2d(x, 20),
					1 => new Point2d(x, 15),
					2 => new Point2d(x, 10),
					3 => new Point2d(x, 5),
					4 => new Point2d(x, 0),
					_ => throw new InvalidOperationException()
				};
			}
		}

		private Point2d GetS2LargerParticlePosition(float time, int particleId)
		{
			if (time < 10)
			{
				return particleId switch
				{
					0 => new Point2d(4 * time, 20),
					1 => new Point2d(4 * time, 15),
					2 => new Point2d(4 * time, 10),
					3 => new Point2d(4 * time, 5),
					4 => new Point2d(4 * time, 0),
					_ => throw new InvalidOperationException()
				};
			}
			else if (time < 18)
			{
				var x = 40 + 4 * (time - 10) - 3 / 16.0 * (time - 10) * (time - 10);
				var xDiff = x - 40;
				return particleId switch
				{
					0 => new Point2d(x, 20 + xDiff * 2),
					1 => new Point2d(x, 15 + xDiff * 1.75),
					2 => new Point2d(x, 10 + xDiff * 1.5),
					3 => new Point2d(x, 5 + xDiff * 1.25),
					4 => new Point2d(x, xDiff),
					_ => throw new InvalidOperationException()
				};
			}
			else
			{
				time = Math.Min(time, 58);
				var x = 60 + (time - 18);
				return particleId switch
				{
					0 => new Point2d(x, 60),
					1 => new Point2d(x, 50),
					2 => new Point2d(x, 40),
					3 => new Point2d(x, 30),
					4 => new Point2d(x, 20),
					_ => throw new InvalidOperationException()
				};
			}
		}
	}
}
