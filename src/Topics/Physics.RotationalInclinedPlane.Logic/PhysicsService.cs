#nullable enable

using System;
using Physics.Shared.Helpers;

namespace Physics.RotationalInclinedPlane.Logic
{
	public class PhysicsService
	{
		public PhysicsService(MotionSetup setup)
		{
			if (setup is null)
			{
				throw new ArgumentNullException(nameof(setup));
			}

			Setup = setup;
		}

		public float AngleInRad => MathHelpers.DegreesToRadians(Setup.InclinedAngle);

		public MotionSetup Setup { get; }

		public float CalculateAcceleration()
		{
			var accelerationCoefficient = GetAccelerationCoefficient();
			return accelerationCoefficient * Setup.Gravity * (float)Math.Sin(AngleInRad);
		}

		public float CalculateVelocity(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var acceleration = CalculateAcceleration();
			return acceleration * timeInSeconds;
		}

		public float CalculateMaxT()
		{
			var acceleration = CalculateAcceleration();
			return (float)Math.Sqrt(Setup.InclinedLength * 2 / acceleration);
		}

		public float CalculateAngularVelocity(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var velocity = CalculateVelocity(timeInSeconds);
			return velocity / Setup.Radius;
		}

		public float CalculateTotalAngleInRad(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var distance = CalculateDistance(timeInSeconds);
			var angleInRad = distance / (Setup.Radius);
			return angleInRad;
		}

		public float CalculateDistance(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var acceleration = CalculateAcceleration();
			return 0.5f * acceleration * timeInSeconds * timeInSeconds;
		}

		public float CalculateX(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var distance = CalculateDistance(timeInSeconds);
			return distance * (float)Math.Cos(AngleInRad);
		}

		public float CalculateY(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var startingHeight = CalculateStartY();
			var distance = CalculateDistance(timeInSeconds);
			return startingHeight - distance * (float)Math.Sin(AngleInRad);
		}

		public float CalculateInclinedWidth()
		{
			return Setup.InclinedLength * (float)Math.Cos(AngleInRad);
		}

		public float CalculateTotalWidth()
		{
			return CalculateInclinedWidth() + Setup.HorizontalLength;
		}

		public float CalculateHorizontalStartX()
		{
			//if (_horizontalStartX == null)
			//{
			//	if (WillReachInclinedEnd())
			//	{
			//		_horizontalStartX = CalculateInclinedX(CalculateInclinedMaxT());
			//	}
			//	else
			//	{
			//		_horizontalStartX = 0;
			//	}
			//}
			//return _horizontalStartX.Value;
			return 0;
		}

		public float CalculateStartY() => Setup.InclinedLength * (float)Math.Sin(AngleInRad);

		public float CalculateEp(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var height = CalculateY(timeInSeconds);
			return Setup.Mass * Setup.Gravity * height;
		}

		public float CalculateEk(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var velocity = CalculateVelocity(timeInSeconds);
			return 0.5f * Setup.Mass * velocity * velocity;
		}

		public float CalculateEr(float timeInSeconds)
		{
			timeInSeconds = Math.Min(timeInSeconds, CalculateMaxT());

			var j = CalculateJ();
			var angular = CalculateAngularVelocity(timeInSeconds);
			return 0.5f * j * angular * angular;
		}

		public float CalculateJ()
		{
			var jCoefficient = GetJCoefficient();
			return jCoefficient * Setup.Mass * Setup.Radius * Setup.Radius;
		}

		public float GetJCoefficient() =>
			Setup.BodyType switch
			{
				BodyType.HollowCylinder => 1,
				BodyType.FullCylinder => 1 / 2.0f,
				BodyType.Sphere => 2 / 5.0f,
				_ => throw new InvalidOperationException("Invalid body type")
			};

		public float GetAccelerationCoefficient() =>
			Setup.BodyType switch
			{
				BodyType.HollowCylinder => 1 / 2.0f,
				BodyType.FullCylinder => 2 / 3.0f,
				BodyType.Sphere => 5 / 7.0f,
				_ => throw new InvalidOperationException("Invalid body type")
			};
	}
}
