using System;
using System.Numerics;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.Shared.Logic.Constants;

namespace Physics.HomogenousMovement.Logic.PhysicsServices
{
	public class MotionInfo
	{
		public MotionInfo(MovementType movementType, Vector2 origin, float mass, float v0, float angle, string color, float g = PhysicsConstants.EarthGravity)
		{
			if (mass <= 0f)
			{
				throw new ArgumentException("WeightMustBeMoreThanZero");
			}

			if (v0 > 0 && v0 / g > 100)
			{
				throw new ArgumentException("InitialSpeedIsTooHigh");
			}

			Type = movementType;
			Mass = mass;
			Origin = origin;
			V0 = v0;
			Angle = angle;
			Color = color;
			G = g;
		}

		public MotionInfo()
		{
		}

		public MotionInfo Clone()
		{
			return new MotionInfo(this.Type, Origin, Mass, V0, Angle, Color, G)
			{
				Label = Label,
			};
		}

		public MovementType Type { get; set; }
		public Vector2 Origin { get; set; }
		public float G { get; set; }
		public float V0 { get; set; }
		public float Mass { get; set; }
		public float Angle { get; set; }
		public string Color { get; set; }
		public string Label { get; set; }
	}
}
