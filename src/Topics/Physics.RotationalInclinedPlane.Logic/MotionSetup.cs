using System;

namespace Physics.RotationalInclinedPlane.Logic
{
	public class MotionSetup
	{
		public MotionSetup(
			string label,
			BodyType bodyType,
			float mass,
			float gravity,
			float radius,
			float inclinedLength,
			float inclinedAngle,
			float horizontalLength,
			string color)
		{
			Label = label;
			BodyType = bodyType;
			Mass = mass;
			InclinedAngle = inclinedAngle;
			InclinedLength = inclinedLength;
			Color = color ?? throw new ArgumentNullException(nameof(color));
			HorizontalLength = horizontalLength;
			Gravity = gravity;
			Radius = radius;
		}

		public string Label { get; set; }

		public BodyType BodyType { get; set; }

		public float Mass { get; set; }

		public bool HasHorizontal => HorizontalLength > 0;

		public float InclinedAngle { get; set; }

		public float InclinedLength { get; set; }

		public float Gravity { get; set; }

		public float Radius { get; set; }

		public float HorizontalLength { get; set; }

		public string Color { get; set; }

		public MotionSetup Clone() =>
			new MotionSetup(Label, BodyType, Mass, Gravity, Radius, InclinedLength, InclinedAngle, HorizontalLength, Color);
	}
}
