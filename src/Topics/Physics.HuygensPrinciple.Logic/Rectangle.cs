using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public class Rectangle : IShape
	{
		public Rectangle(PointF topLeft, PointF bottomRight, CellState state = CellState.Source)
		{
			TopLeft = topLeft;
			BottomRight = bottomRight;
			State = state;
		}

		public PointF TopLeft { get; }

		public PointF BottomRight { get; }

		public CellState State { get; }

		public void Draw(HuygensField field)
		{
			var width = field.Width;
			var height = field.Height;

			var top = height * TopLeft.Y;
			var bottom = height * BottomRight.Y;
			var left = width * TopLeft.X;
			var right = width * BottomRight.X;

			HuygensShapeDrawer.DrawRectangle(field, new Point((int)left, (int)top), new Point((int)right, (int)bottom), State, false);
		}

		public bool HitTest(double relativeX, double relativeY)
		{
			return
				TopLeft.X <= relativeX && relativeX <= BottomRight.X &&
				TopLeft.Y <= relativeY && relativeY <= BottomRight.Y;
		}
	}
}
