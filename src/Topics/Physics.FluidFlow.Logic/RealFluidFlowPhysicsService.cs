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

		public float XMax => 0.02f;

		public float YMax => 0.5f * _input.Diameter1;

		public float YMin => -0.5f * _input.Diameter1;

		public float R => 1000000 * _input.Velocity * _input.Diameter1;

		public bool CanRenderFlow => R < 2300;

		public string ErrorKey => "ErrorRealFluidLowerDOrV";

		public float DeltaP => 0.320f * _input.Length * _input.Velocity / (_input.Diameter1 * _input.Diameter1);

		public float Vector1T => MaxT / 6;

		public float Vector2T => MaxT * 5 / 6f;

		public float V2 => _input.Velocity;

		public float P2 => 0;

		public float H1 => 0;

		public float H2 => 0;

		public float SimulationTimeAdjustment => 0.2f;

		public Point2d GetParticlePosition(float time, int particleId)
		{
			time = time / 70;
			return particleId switch
			{
				0 => new Point2d(0 * time, _input.Diameter1 / 2),
				1 => new Point2d(7 / 8f * _input.Velocity * time, 3 / 8f * _input.Diameter1),
				2 => new Point2d(3 / 2f * _input.Velocity * time, 2 / 8f * _input.Diameter1),
				3 => new Point2d(15 / 8f * _input.Velocity * time, 1 / 8f * _input.Diameter1),
				4 => new Point2d(2 / 1f * _input.Velocity * time, 0),
				5 => new Point2d(15 / 8f * _input.Velocity * time, -1 / 8f * _input.Diameter1),
				6 => new Point2d(3 / 2f * _input.Velocity * time, -2 / 8f * _input.Diameter1),
				7 => new Point2d(7 / 8f * _input.Velocity * time, -3 / 8f * _input.Diameter1),
				8 => new Point2d(0 * time, -_input.Diameter1 / 2),
				_ => throw new InvalidOperationException()
			};
		}

		public float MiddleParticleEndTime => XMax / (2 / 1f * _input.Velocity) * 70;
	}
}
