using System;
using System.Numerics;

namespace Physics.DragMovement.Logic.PhysicsServices
{
	public enum MovementType
	{
		FreeFall,
		ProjectileMotion
	}

	public class MotionInfo
	{
		private float _mass = -1;
		public float _area = -1;

		public MotionInfo()
		{
		}

		public MotionInfo(MovementType movementType, Vector2 origin, float resistance, bool isBall, float mass, float area, float originSpeed, float elevationAngle, float gravity, float environmentDensity, float diameter, float shapeDensity, string color)
		{
			if (isBall)
			{
				if (shapeDensity < 100)
				{
					throw new ArgumentException("DensityMustBeMoreThanHundred");
				}
			}
			else
			{
				if (mass <= 0f)
				{
					throw new ArgumentException("WeightMustBeMoreThanZero");
				}
			}

			if (originSpeed > 0 && originSpeed / gravity > 100)
			{
				throw new ArgumentException("InitialSpeedIsTooHigh");
			}

			IsBall = isBall;
			Type = movementType;
			Origin = origin;
			Resistance = resistance;
			_mass = mass;
			_area = area;
			OriginSpeed = originSpeed;
			ElevationAngle = elevationAngle;
			G = gravity;
			EnvironmentDensity = environmentDensity;
			Diameter = diameter;
			ShapeDensity = shapeDensity;
			Color = color;
		}

		public MotionInfo Clone()
		{
			return new MotionInfo(this.Type, Origin, Resistance, IsBall, Mass, Area, OriginSpeed, ElevationAngle, G, EnvironmentDensity, Diameter, ShapeDensity, Color)
			{
				Label = Label
			};
		}

		public MovementType Type { get; set; }
		public bool IsBall { get; set; }
		public Vector2 Origin { get; set; }
		public float Resistance { get; set; }

		public float Mass => !IsBall ?
			_mass : ShapeDensity * (4 / 3.0f * (float)Math.PI * (float)Math.Pow(Diameter / 2, 3));

		public float Area => !IsBall ?
			_area : (float)Math.PI * (float)Math.Pow(Diameter / 2, 2);

		public float OriginSpeed { get; set; }
		public float ElevationAngle { get; set; }
		public float G { get; set; }
		public float EnvironmentDensity { get; set; }
		public float Diameter { get; set; }
		public float ShapeDensity { get; set; }
		public string Color { get; set; }
		public string Label { get; set; }
	}
}
