using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace Physics.Shared.UI.Rendering.Skia
{
	public static class ArrowRenderer
	{
		public static void Draw(
			SKCanvas canvas,
			SKPoint from,
			SKPoint to,
			float tipSize,
			SKPaint paint)
		{
			if (from == to)
			{
				return;
			}

			canvas.DrawLine(from, to, paint);

			var angle = Math.Atan2(to.Y - from.Y, to.X - from.X);
			DrawArrowTip(canvas, to, angle - (float)Math.PI, tipSize, paint);
		}

		public static void Draw(
			SKSurface surface,
			SKPoint from,
			SKPoint to,
			float tipSize,
			SKPaint paint) => Draw(surface.Canvas, from, to, tipSize, paint);

		public static void Draw(
			SKCanvas canvas,
			SKPoint from,
			float length,
			float angle,
			float tipSize,
			SKPaint paint)
		{
			if (length == 0)
			{
				return;
			}

			var to = new SKPoint(
				(float)(from.X + Math.Cos(angle) * length),
				(float)(from.Y + Math.Sin(angle) * length));

			Draw(canvas, from, to, tipSize, paint);
		}

		public static void Draw(
			SKSurface surface,
			SKPoint from,
			float length,
			float angle,
			float tipSize,
			SKPaint paint) => Draw(surface.Canvas, from, length, angle, tipSize, paint);

		private static void DrawArrowTip(
			SKCanvas canvas,
			SKPoint point,
			double angle,
			float tipSize,
			SKPaint paint)
		{
			// Calculate unit vector of the given angle
			var directionX = Math.Cos(angle);
			var directionY = Math.Sin(angle);
			SKPoint v = new SKPoint((float)directionX * tipSize, (float)directionY * tipSize);
			// Invert it
			// Move point back the given distance
			var pointMovedBack = point + v;
			// Calculate perpendicular vector
			var vectorPerpendicular = new SKPoint(v.Y, -v.X);
			// calculate head ends
			var firstPoint = pointMovedBack + new SKPoint(vectorPerpendicular.X * 1, vectorPerpendicular.Y * 1);
			var secondPoint = pointMovedBack - new SKPoint(vectorPerpendicular.X * 1, vectorPerpendicular.Y * 1);
			// draw arrow lines
			canvas.DrawLine(firstPoint, point, paint);
			canvas.DrawLine(secondPoint, point, paint);
		}
	}
}
