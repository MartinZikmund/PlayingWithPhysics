using System;

namespace Physics.Shared.Logic.Geometry
{
	public static class IntersectionHelpers
	{
		public static Point2d[] FindLineCircleIntersections(
			float cx, float cy, float radius,
			Point2d point1, Point2d point2)
		{
			double dx, dy, A, B, C, det, t;

			dx = point2.X - point1.X;
			dy = point2.Y - point1.Y;

			A = dx * dx + dy * dy;
			B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
			C = (point1.X - cx) * (point1.X - cx) +
				(point1.Y - cy) * (point1.Y - cy) -
				radius * radius;

			det = B * B - 4 * A * C;
			if ((A <= 0.0000001) || (det < 0))
			{
				// No real solutions.
				return Array.Empty<Point2d>();
			}
			else if (det == 0)
			{
				// One solution.
				t = -B / (2 * A);
				return new Point2d[] {
					new Point2d(point1.X + t * dx, point1.Y + t * dy)
				};
			}
			else
			{
				// Two solutions.
				t = (float)((-B + Math.Sqrt(det)) / (2 * A));
				var intersection1 =
					new Point2d(point1.X + t * dx, point1.Y + t * dy);
				t = (float)((-B - Math.Sqrt(det)) / (2 * A));
				var intersection2 =
					new Point2d(point1.X + t * dx, point1.Y + t * dy);
				return new Point2d[]
				{
					intersection1,
					intersection2
				};
			}
		}
	}
}
