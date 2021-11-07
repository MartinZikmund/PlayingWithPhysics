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

		public void Draw(HuygensField field)
		{
			var width = field.Width;
			var height = field.Height;

			var centerX = width * Center.X;
			var centerY = height * Center.Y;

			var dimension = Math.Min(width, height);
			var radius = Radius * dimension;

			HuygensShapeDrawer.DrawCircle(field, new Point((int)centerX, (int)centerY), radius);
		}
	}
}
