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

		public int ParticleCount => 5;

		public float XMax => 0.2f;

		public float YMax => 9 / 16f * _input.Diameter1;

		public float YMin => -9 / 16f * _input.Diameter1;

		public float R => 1000000 * _input.Velocity * _input.Diameter1;

		public bool CanRenderFlow => R < 2300;

		public string ErrorKey => "ErrorRealFluidLowerDOrV";

		public float DeltaP => 320 * _input.Length * _input.Velocity / (_input.Diameter1 * _input.Diameter1);

		public float Vector1T => MaxT / 6;

		public float Vector2T => MaxT * 5 / 6f;

		public float SimulationTimeAdjustment => 0.2f;

		public Point2d GetParticlePosition(float time, int particleId) =>
			particleId switch
			{
				0 => new Point2d(0 * time, _input.Diameter1 / 2),
				1 => new Point2d(3 / 2f * _input.Velocity * time, 2 / 8f * _input.Diameter1),
				2 => new Point2d(2 / 1f * _input.Velocity * time, 0),
				3 => new Point2d(3 / 2f * _input.Velocity * time, -2 / 8f * _input.Diameter1),
				4 => new Point2d(0 * time, -_input.Diameter1 / 2),
				_ => throw new InvalidOperationException()
			};
	}
}
