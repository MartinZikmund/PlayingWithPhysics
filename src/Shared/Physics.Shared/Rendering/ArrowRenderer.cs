using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.UI;

namespace Physics.Shared.UI.Rendering
{
    public static class ArrowRenderer
    {
        public static void Draw(
            Vector2 from,
            float length,
            float angle,
            Color color,
            ICanvasAnimatedControl sender, 
            CanvasDrawingSession session)
        {
            var to = new Vector2(
                (float)(from.X + Math.Cos(angle) * length),
                (float)(from.Y + Math.Sin(angle) * length));

            session.DrawLine(from, to, color);

            DrawArrowTip(to, angle - (float)Math.PI, color, session);
        }

        private static void DrawArrowTip(Vector2 point, double angle, Color color, CanvasDrawingSession session)
        {
            // Calculate unit vector of the given angle
            var directionX = Math.Cos(angle);
            var directionY = Math.Sin(angle);
            Vector2 v = new Vector2((float)directionX, (float)directionY);
            // Invert it
            // Move point back the given distance
            var pointMovedBack = point + v * 10;
            // Calculate perpendicular vector
            var vectorPerpendicular = new Vector2(v.Y, -v.X);
            // calculate head ends
            var firstPoint = pointMovedBack + vectorPerpendicular * 5;
            var secondPoint = pointMovedBack - vectorPerpendicular * 5;
            // draw arrow lines
            session.DrawLine(firstPoint, point, color, 2f);
            session.DrawLine(secondPoint, point, color, 2f);
        }
    }
}
