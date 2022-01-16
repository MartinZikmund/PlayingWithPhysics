using System;
using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public class Circle : IShape
	{
		public Circle(PointF center, float radius, CellState state = CellState.Source)
		{
			Center = center;
			Radius = radius;
			State = state;
		}

		public PointF Center { get; }

		public float Radius { get; }

		public CellState State { get; }

		public void Draw(HuygensField field)
		{
			var width = field.Width;
			var height = field.Height;

			var centerX = width * Center.X;
			var centerY = height * Center.Y;

			var dimension = Math.Min(width, height);
			var radius = Radius * dimension;

			HuygensShapeDrawer.DrawCircle(field, new Point((int)centerX, (int)centerY), radius, State, false);
		}

		public bool HitTest(double relativeX, double relativeY)
		{
			var distX = Math.Abs(relativeX - Center.X);
			var distY = Math.Abs(relativeY - Center.Y);
			return Math.Sqrt(distX * distX + distY * distY) <= Radius;
		}
	}
}
