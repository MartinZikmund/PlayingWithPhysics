using System;
using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic
{
	public class ContinuityEquationPhysicsService : IPhysicsService
	{
		private readonly SceneConfiguration _input;

		public ContinuityEquationPhysicsService(SceneConfiguration input)
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
				DiameterRelationType.Equal => _input.YMax,
				DiameterRelationType.S1Larger => 20,
				DiameterRelationType.S2Larger => 20,
				_ => throw new InvalidOperationException("Invalid diameter type"),
			};

		public Point2d GetParticlePosition(float time, int particleId) =>
			_input.DiameterRelationType switch
			{
				DiameterRelationType.Equal => GetDiameterEqualParticlePosition(time, particleId),
				DiameterRelationType.S1Larger => GetS1LargerParticlePosition(time, particleId),
				DiameterRelationType.S2Larger => GetS2LargerParticlePosition(time, particleId),
				_ => throw new InvalidOperationException("Invalid diameter type"),
			};

		private Point2d GetDiameterEqualParticlePosition(float time, int particleId)
		{
			time = Math.Min(time, 58);
			return particleId switch
			{
				0 => new Point2d(_input.Velocity * time, _input.YMax),
				1 => new Point2d(_input.Velocity * time, 2 / 3.0 * _input.YMax),
				2 => new Point2d(_input.Velocity * time, 1 / 3.0 * _input.YMax),
				3 => new Point2d(_input.Velocity * time, 0),
				4 => new Point2d(_input.Velocity * time, -1 / 3.0 * _input.YMax),
				5 => new Point2d(_input.Velocity * time, -2 / 3.0 * _input.YMax),
				6 => new Point2d(_input.Velocity * time, -_input.YMax),
				_ => throw new InvalidOperationException()
			};
		}

		private Point2d GetS1LargerParticlePosition(float time, int particleId)
		{
			if (time < 40)
			{
				return particleId switch
				{
					0 => new Point2d(time, 20),
					1 => new Point2d(time, 10),
					2 => new Point2d(time, 0),
					3 => new Point2d(time, -10),
					4 => new Point2d(time, -20),
					_ => throw new InvalidOperationException()
				};
			}
			else if (time < 48)
			{
				var x = 40 + (time - 40) + 3 / 16.0 * (time - 40) * (time - 40);
				var xDiff = x - 40;
				return particleId switch
				{
					0 => new Point2d(x, 20 - xDiff * 0.5),
					1 => new Point2d(x, 10 - xDiff * 0.25),
					2 => new Point2d(x, 0),
					3 => new Point2d(x, -10 + xDiff * 0.25),
					4 => new Point2d(x, -20 + xDiff * 0.5),
					_ => throw new InvalidOperationException()
				};
			}
			else
			{
				time = Math.Min(time, 58);
				var x = 60 + 4 * (time - 48);
				return particleId switch
				{
					0 => new Point2d(x, 10),
					1 => new Point2d(x, 5),
					2 => new Point2d(x, 0),
					3 => new Point2d(x, -5),
					4 => new Point2d(x, -10),
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
					0 => new Point2d(4 * time, 10),
					1 => new Point2d(4 * time, 5),
					2 => new Point2d(4 * time, 0),
					3 => new Point2d(4 * time, -5),
					4 => new Point2d(4 * time, -10),
					_ => throw new InvalidOperationException()
				};
			}
			else if (time < 18)
			{
				var x = 40 + 4 * (time - 10) - 3 / 16.0 * (time - 10) * (time - 10);
				var xDiff = x - 40;
				return particleId switch
				{
					0 => new Point2d(x, 10 + xDiff * 0.5),
					1 => new Point2d(x, 5 + xDiff * 0.25),
					2 => new Point2d(x, 0),
					3 => new Point2d(x, -5 - xDiff * 0.25),
					4 => new Point2d(x, -10 - xDiff * 0.5),
					_ => throw new InvalidOperationException()
				};
			}
			else
			{
				time = Math.Min(time, 58);
				var x = 60 + 4 * (time - 18);
				return particleId switch
				{
					0 => new Point2d(x, 20),
					1 => new Point2d(x, 10),
					2 => new Point2d(x, 0),
					3 => new Point2d(x, -10),
					4 => new Point2d(x, -20),
					_ => throw new InvalidOperationException()
				};
			}
		}
	}
}
