using System;
using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic
{
	public class RealFluidFlowPhysicsService : PhysicsServiceBase, IPhysicsService
	{
		private readonly SceneConfiguration _input;

		public RealFluidFlowPhysicsService(SceneConfiguration input)
		{
			_input = input;
		}

		public int ParticleCount => 9;

		public float XMax =>
			_input.DiameterRelationType switch
			{
				DiameterRelationType.Equal => 58 * _input.Velocity,
				DiameterRelationType.S1Larger => 100,
				DiameterRelationType.S2Larger => 100,
				_ => throw new InvalidOperationException("Invalid diameter type"),
			};

		public float YMax => 0.4f;

		public float YMin => -0.4f;

		public float R => 1000000 * _input.Velocity * _input.Diameter1;

		public bool CanRenderFlow => R < 2300;

		public string ErrorKey => "ErrorRealFluidLowerDOrV";

		public float DeltaP => 320 * _input.Length * _input.Velocity / (_input.Diameter1 * _input.Diameter1);

		public Point2d GetParticlePosition(float time, int particleId) =>
			particleId switch
			{
				0 => new Point2d(0 * time, YMax),
				1 => new Point2d(7 / 8f * _input.Velocity * time, 3 / 4f * YMax),
				2 => new Point2d(3 / 2f * _input.Velocity * time, 2 / 4f * YMax),
				3 => new Point2d(15 / 8f * _input.Velocity * time, 1 / 4f * YMax),
				4 => new Point2d(2 / 1f * _input.Velocity * time, 0),
				5 => new Point2d(15 / 8f * _input.Velocity * time, -1 / 4f * YMax),
				6 => new Point2d(3 / 2f * _input.Velocity * time, -2 / 4f * YMax),
				7 => new Point2d(7 / 8f * _input.Velocity * time, -3 / 4f * YMax),
				8 => new Point2d(0 * time, -YMax),
				_ => throw new InvalidOperationException()
			};
	}
}
