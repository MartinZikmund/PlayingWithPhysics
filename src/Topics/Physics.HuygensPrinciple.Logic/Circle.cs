using System;
using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public class Circle : IShape
	{
		public Circle(PointF center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public PointF Center { get; }

		public float Radius { get; }

		public void Render(HuygensStepper stepper)
		{
			var width = stepper.FieldWidth;
			var height = stepper.FieldHeight;

			var centerX = width * Center.X;
			var centerY = height * Center.Y;

			var dimension = Math.Min(width, height);
			var radius = Radius * dimension;

			stepper.PutCircle(new Point((int)centerX, (int)centerY), radius);
		}
	}
}
