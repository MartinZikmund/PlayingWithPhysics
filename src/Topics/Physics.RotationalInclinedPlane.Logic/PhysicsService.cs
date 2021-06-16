#nullable enable

using System;
using Physics.Shared.Helpers;

namespace Physics.RotationalInclinedPlane.Logic
{
	public class PhysicsService
	{
		private readonly MotionSetup _setup;

		public PhysicsService(MotionSetup setup)
		{
			if (setup is null)
			{
				throw new ArgumentNullException(nameof(setup));
			}

			_setup = setup;
		}

		public float AngleInRad => MathHelpers.DegreesToRadians(_setup.InclinedAngle);

		public float CalculateAcceleration()
		{
			var accelerationCoefficient = GetAccelerationCoefficient();
			return accelerationCoefficient * _setup.Gravity * (float)Math.Sin(AngleInRad);
		}

		public float CalculateVelocity(float timeInSeconds)
		{
			var acceleration = CalculateAcceleration();
			return acceleration * timeInSeconds;
		}

		public float CalculateMaxT()
		{
			var acceleration = CalculateAcceleration();
			return (float)Math.Sqrt(_setup.InclinedLength * 2 / acceleration);
		}

		public float CalculateAngularVelocity(float timeInSeconds)
		{
			var velocity = CalculateVelocity(timeInSeconds);
			return velocity / _setup.Radius;
		}

		public float CalculateDistance(float timeInSeconds)
		{
			var acceleration = CalculateAcceleration();
			return 0.5f * acceleration * timeInSeconds * timeInSeconds;
		}

		public float CalculateX(float timeInSeconds)
		{
			var distance = CalculateDistance(timeInSeconds);
			return distance * (float)Math.Cos(AngleInRad);
		}

		public float CalculateY(float timeInSeconds)
		{
			var startingHeight = CalculateStartY();
			var distance = CalculateDistance(timeInSeconds);
			return startingHeight - distance * (float)Math.Sin(_setup.InclinedAngle);
		}

		public float CalculateStartY() => _setup.InclinedLength * (float)Math.Sin(AngleInRad);

		public float CalculateEp(float timeInSeconds)
		{
			var height = CalculateY(timeInSeconds);
			return _setup.Mass * _setup.Gravity * height;
		}

		public float CalculateEk(float timeInSeconds)
		{
			var velocity = CalculateVelocity(timeInSeconds);
			return 0.5f * _setup.Mass * velocity;
		}

		public float CalculateEr(float timeInSeconds)
		{
			var j = CalculateJ();
			return 0.5f * j * CalculateAngularVelocity(timeInSeconds);
		}

		public float CalculateJ()
		{
			var jCoefficient = GetJCoefficient();
			return jCoefficient * _setup.Mass * _setup.Radius * _setup.Radius;
		}

		public float GetJCoefficient() =>
			_setup.BodyType switch
			{
				BodyType.HollowCylinder => 1,
				BodyType.FullCylinder => 1 / 2.0f,
				BodyType.Sphere => 2 / 5.0f,
				_ => throw new InvalidOperationException("Invalid body type")
			};

		public float GetAccelerationCoefficient() =>
			_setup.BodyType switch
			{
				BodyType.HollowCylinder => 1 / 2.0f,
				BodyType.FullCylinder => 2 / 3.0f,
				BodyType.Sphere => 5 / 7.0f,
				_ => throw new InvalidOperationException("Invalid body type")
			};
	}
}
