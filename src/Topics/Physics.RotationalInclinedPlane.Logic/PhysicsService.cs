#nullable enable

using System;
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Constants;

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

		public double CalculateAcceleration()
		{
			var accelerationCoefficient = GetAccelerationCoefficient();
			return accelerationCoefficient * _setup.Gravity * Math.Sin(AngleInRad);
		}

		public double CalculateVelocity(double timeInSeconds)
		{
			var acceleration = CalculateAcceleration();
			return acceleration * timeInSeconds;
		}

		public double CalculateAngularVelocity(double timeInSeconds)
		{
			var velocity = CalculateVelocity(timeInSeconds);
			return velocity / _setup.Radius;
		}

		public double CalculateDistance(double timeInSeconds)
		{
			var acceleration = CalculateAcceleration();
			return 0.5 * acceleration * timeInSeconds * timeInSeconds;
		}

		public double CalculateX(double timeInSeconds)
		{
			var distance = CalculateDistance(timeInSeconds);
			return distance * Math.Cos(AngleInRad);
		}

		public double CalculateY(double timeInSeconds)
		{
			var startingHeight = CalculateStartY();
			var distance = CalculateDistance(timeInSeconds);
			return startingHeight - distance * Math.Sin(_setup.InclinedAngle);
		}

		public double CalculateStartY() => _setup.InclinedLength * (float)Math.Sin(AngleInRad);

		public double CalculateEp(double timeInSeconds)
		{
			var height = CalculateY(timeInSeconds);
			return _setup.Mass * _setup.Gravity * height;
		}

		public double CalculateEk(double timeInSeconds)
		{
			var velocity = CalculateVelocity(timeInSeconds);
			return 0.5 * _setup.Mass * velocity;
		}

		public double CalculateEr(double timeInSeconds)
		{
			var j = CalculateJ();
			return 0.5 * j * CalculateAngularVelocity(timeInSeconds);
		}

		public double CalculateJ()
		{
			var jCoefficient = GetJCoefficient();
			return jCoefficient * _setup.Mass * _setup.Radius * _setup.Radius;
		}

		public double GetJCoefficient() =>
			_setup.BodyType switch
			{
				BodyType.HollowCylinder => 1,
				BodyType.FullCylinder => 1 / 2.0,
				BodyType.Sphere => 2 / 5.0,
				_ => throw new InvalidOperationException("Invalid body type")
			};

		public double GetAccelerationCoefficient() =>
			_setup.BodyType switch
			{
				BodyType.HollowCylinder => 1 / 2.0,
				BodyType.FullCylinder => 2 / 3.0,
				BodyType.Sphere => 5 / 7.0,
				_ => throw new InvalidOperationException("Invalid body type")
			};
	}
}
