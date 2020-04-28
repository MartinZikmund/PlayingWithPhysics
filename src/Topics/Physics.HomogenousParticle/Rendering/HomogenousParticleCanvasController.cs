using MathNet.Spatial.Euclidean;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.Shared.Helpers;
using Physics.Shared.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Color = Windows.UI.Color;

namespace Physics.HomogenousParticle.Rendering
{
    public abstract class HomogenousParticleCanvasControllerBase : BaseCanvasController, IDisposable
    {
        private const int ArrowDistance = 20;

        protected int SimulationPadding { get; set; } = 52;

        protected RectangleF _simulationBoundsInMeters = new RectangleF();
        private RectangleF _simulationBoundsInPixels = new RectangleF();

        public HomogenousParticleCanvasControllerBase(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public override void Update(ICanvasAnimatedControl sender)
        {
            _simulationBoundsInPixels = new RectangleF(
                SimulationPadding,
                SimulationPadding,
                (float)sender.Size.Width - SimulationPadding * 2,
                (float)sender.Size.Height - SimulationPadding * 2);
        }

        protected void DrawInductionArrows(double angle, Windows.UI.Color color, CanvasAnimatedDrawEventArgs args)
        {
            // calculate perpendicular line through the simulation center
            var center = new Point2D(_canvasAnimatedControl.Size.Width / 2f, _canvasAnimatedControl.Size.Height / 2f);
            var perpendicularAngle = angle + 90;
            var perpendicularLine = CalculatePointLineUnderAngle(perpendicularAngle, center);
            var normalized = perpendicularLine.Direction;
            int i = 1;
            while (i <= 10)
            {
                var upX = center.X + normalized.X * i * ArrowDistance;
                var upY = center.X + normalized.Y * i * ArrowDistance;

                var upLine = CalculatePointLineUnderAngle(angle, new Point2D(upX, upY));
                DrawInductionArrow(upLine, angle, color, args);

                var inverseX = center.X - normalized.X * i * ArrowDistance;
                var inverseY = center.Y - normalized.Y * i * ArrowDistance;

                var inverseLine = CalculatePointLineUnderAngle(angle, new Point2D(inverseX, inverseY));
                DrawInductionArrow(inverseLine, angle, color, args);

                i++;           
            }
            //for (int i = 1; i < ArrowCount; i++)
            //{
            //    var relativePosition = i / (ArrowCount * 1.0f);
            //    var resultX = _simulationBoundsInPixels.Left + diagonalDirection.X * relativePosition;
            //    var resultY = _simulationBoundsInPixels.Bottom + diagonalDirection.Y * relativePosition;
            //    args.DrawingSession.DrawCircle(new Vector2(resultX, resultY), 5, color);

            //    var line = CalculateLine(resultX, resultY);
            //    var intersectionBottom = line.IntersectWith(new Line2D(new Point2D(SimulationPadding, SimulationPadding), new Point2D(SimulationPadding + 1, SimulationPadding)));
            //    var intersectionTop = line.IntersectWith(new Line2D(new Point2D(0, _simulationBoundsInPixels.Bottom), new Point2D(1, _simulationBoundsInPixels.Bottom)));
            //    if (intersectionBottom != null && intersectionTop != null)
            //        args.DrawingSession.DrawLine(new Vector2((float)intersectionBottom.Value.X, (float)intersectionBottom.Value.Y), new Vector2((float)intersectionTop.Value.X, (float)intersectionTop.Value.Y), color);

            //}
            //      calculate intersection with left, right, top and bottom boundary
            //      select the two intersections within canvas boundary segments
            //      draw line
            //      TODO: draw arrows
        }

        private void DrawInductionArrow(Line2D line, double angle, Color color, CanvasAnimatedDrawEventArgs args)
        {
            var intersectionBottom = line.IntersectWith(new Line2D(new Point2D(SimulationPadding, SimulationPadding), new Point2D(SimulationPadding + 1, SimulationPadding)));
            var intersectionTop = line.IntersectWith(new Line2D(new Point2D(0, _simulationBoundsInPixels.Bottom), new Point2D(1, _simulationBoundsInPixels.Bottom)));
            if (intersectionBottom != null && intersectionTop != null)
                args.DrawingSession.DrawLine(new Vector2((float)intersectionBottom.Value.X, (float)intersectionBottom.Value.Y), new Vector2((float)intersectionTop.Value.X, (float)intersectionTop.Value.Y), color);
        }

        private Line2D CalculatePointLineUnderAngle(double angle, Point2D point)
        {
            var a = Math.Tan(MathHelpers.DegreesToRadians((float)angle));
            var b = point.Y - a * point.X;

            var x1 = point.X + 1;
            var y1 = a * x1 + b;

            return new Line2D(new Point2D(point.X, point.Y), new Point2D(x1, y1));
        }
    }
}
