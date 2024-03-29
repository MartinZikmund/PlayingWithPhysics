﻿using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Services;
using Physics.Shared.Helpers;
using Physics.Shared.Logic.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Physics.Shared.UI.Rendering;
using Color = Windows.UI.Color;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class HomogenousParticleCanvasController : BaseCanvasController
    {
        private const int ArrowDistance = 80;
        private const int InductionDistance = 40;

        protected int SimulationPadding { get; set; } = 52;

        protected SimulationBounds _simulationBoundsInMeters = new SimulationBounds();
        private SimulationBounds _simulationBoundsInPixels = new SimulationBounds();

        private IVariantRenderer _renderer;

        public HomogenousParticleCanvasController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public SimulationBounds SimulationBoundsInPixels => _simulationBoundsInPixels;

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            return Task.CompletedTask;
        }

        public void SetVariantRenderer(IVariantRenderer renderer)
        {
            _renderer = renderer;
        }

        public void StartSimulation(IMotionSetup[] motions)
        {
            _renderer?.StartSimulation(motions);
            SimulationTime.Restart();
        }

        public override void Update(ICanvasAnimatedControl sender)
        {
            _simulationBoundsInPixels = new SimulationBounds(
                SimulationPadding,
                SimulationPadding,
                (float)sender.Size.Width - SimulationPadding,
                (float)sender.Size.Height - SimulationPadding);

            _renderer?.Update(sender);
        }

        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            _renderer?.Draw(sender, args);
        }

        internal void DrawInductionArrows(double angle, Windows.UI.Color color, CanvasAnimatedDrawEventArgs args)
        {
            var semiTransparentColor = Color.FromArgb(125, color.R, color.G, color.B);
            // calculate perpendicular line through the simulation center
            var center = new Point2d((float)_canvasAnimatedControl.Size.Width / 2f, (float)_canvasAnimatedControl.Size.Height / 2f);
            var perpendicularAngle = angle - 90;
            var perpendicularLine = CalculatePointLineUnderAngle(perpendicularAngle, center);
            var normalized = perpendicularLine.Direction;
            int i = 0;
            while (i <= 10)
            {
                var upX = center.X + normalized.X * i * ArrowDistance;
                var upY = center.Y + normalized.Y * i * ArrowDistance;

                var upLine = CalculatePointLineUnderAngle(angle, new Point2d(upX, upY));
                DrawInductionArrow(upLine, angle, semiTransparentColor, args);

                if (i != 0)
                {
                    var inverseX = center.X - normalized.X * i * ArrowDistance;
                    var inverseY = center.Y - normalized.Y * i * ArrowDistance;

                    var inverseLine = CalculatePointLineUnderAngle(angle, new Point2d(inverseX, inverseY));
                    DrawInductionArrow(inverseLine, angle, semiTransparentColor, args);
                }

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

        private void DrawInductionArrow(Line2d line, double angle, Color color, CanvasAnimatedDrawEventArgs args)
        {
            angle = ((angle % 360.0) + 360) % 360.0;
            var touchPoints = new List<Point2d>();

            var intersectionBottom = line.IntersectWith(new Line2d(new Point2d(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Bottom), new Point2d(_simulationBoundsInPixels.Left + 1, _simulationBoundsInPixels.Bottom)));
            if (intersectionBottom != null && _simulationBoundsInPixels.Left <= intersectionBottom.Value.X && intersectionBottom.Value.X <= _simulationBoundsInPixels.Right)
            {
                touchPoints.Add(intersectionBottom.Value);
            }

            var intersectionTop = line.IntersectWith(new Line2d(new Point2d(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Top), new Point2d(_simulationBoundsInPixels.Left + 1, _simulationBoundsInPixels.Top)));
            if (intersectionTop != null && _simulationBoundsInPixels.Left <= intersectionTop.Value.X && intersectionTop.Value.X <= _simulationBoundsInPixels.Right)
            {
                touchPoints.Add(intersectionTop.Value);
            }

            var intersectionLeft = line.IntersectWith(new Line2d(new Point2d(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Bottom), new Point2d(_simulationBoundsInPixels.Left, _simulationBoundsInPixels.Bottom + 1)));
            if (intersectionLeft != null && intersectionLeft.Value.Y != _simulationBoundsInPixels.Bottom && intersectionLeft.Value.Y != _simulationBoundsInPixels.Top &&
                 _simulationBoundsInPixels.Bottom >= intersectionLeft.Value.Y && intersectionLeft.Value.Y >= _simulationBoundsInPixels.Top)
            {
                touchPoints.Add(intersectionLeft.Value);
            }
            var intersectionRight = line.IntersectWith(new Line2d(new Point2d(_simulationBoundsInPixels.Right, _simulationBoundsInPixels.Bottom), new Point2d(_simulationBoundsInPixels.Right, _simulationBoundsInPixels.Bottom + 1)));
            if (intersectionRight != null && intersectionRight.Value.Y != _simulationBoundsInMeters.Bottom && intersectionRight.Value.Y != _simulationBoundsInPixels.Top &&
                 _simulationBoundsInPixels.Bottom >= intersectionRight.Value.Y && intersectionRight.Value.Y >= _simulationBoundsInPixels.Top)
            {
                touchPoints.Add(intersectionRight.Value);
            }

            if (touchPoints.Count == 2)
            {
                var firstPoint = new Vector2((float)touchPoints[0].X, (float)touchPoints[0].Y);
                var secondPoint = new Vector2((float)touchPoints[1].X, (float)touchPoints[1].Y);
                args.DrawingSession.DrawLine(firstPoint, secondPoint, color, 2f);

                Vector2 arrowPoint;
                if (angle > 270 || angle < 90)
                {
                    // the arrow is pointing to the "right"
                    if (firstPoint.X > secondPoint.X)
                    {
                        arrowPoint = firstPoint;
                    }
                    else
                    {
                        arrowPoint = secondPoint;
                    }
                }
                else if (angle > 90 && angle < 270)
                {
                    if (firstPoint.X < secondPoint.X)
                    {
                        arrowPoint = firstPoint;
                    }
                    else
                    {
                        arrowPoint = secondPoint;
                    }
                }
                else if (angle == 90)
                {
                    if (firstPoint.Y < secondPoint.Y)
                    {
                        arrowPoint = firstPoint;
                    }
                    else
                    {
                        arrowPoint = secondPoint;
                    }
                }
                else
                {
                    if (firstPoint.Y > secondPoint.Y)
                    {
                        arrowPoint = firstPoint;
                    }
                    else
                    {
                        arrowPoint = secondPoint;
                    }
                }

                DrawArrowTip(arrowPoint, angle, color, args);
            }
        }

        internal void DrawParticle(Vector2 position, Color color, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.FillCircle(position, 15, color);
        }

        private void DrawArrowTip(Vector2 point, double angle, Color color, CanvasAnimatedDrawEventArgs args)
        {
            // Calculate unit vector of the given angle
            var directionX = Math.Cos(MathHelpers.DegreesToRadians((float)angle));
            var directionY = -Math.Sin(MathHelpers.DegreesToRadians((float)angle));
            Vector2 v = new Vector2((float)directionX, (float)directionY);
            // Invert it
            v = -v;
            // Move point back the given distance
            var pointMovedBack = point + v * 10;
            // Calculate perpendicular vector
            var vectorPerpendicular = new Vector2(v.Y, -v.X);
            // calculate head ends
            var firstPoint = pointMovedBack + vectorPerpendicular * 5;
            var secondPoint = pointMovedBack - vectorPerpendicular * 5;
            // draw arrow lines
            args.DrawingSession.DrawLine(firstPoint, point, color, 2f);
            args.DrawingSession.DrawLine(secondPoint, point, color, 2f);
        }

        private Line2d CalculatePointLineUnderAngle(double angle, Point2d point)
        {
            var a = Math.Tan(MathHelpers.DegreesToRadians(-(float)angle));
            var b = point.Y - a * point.X;

            var x1 = point.X + 1;
            var y1 = (float)a * x1 + (float)b;

            return new Line2d(new Point2d(point.X, point.Y), new Point2d(x1, y1));
        }

        internal void DrawInductionGrid(
            ICanvasAnimatedControl sender,
            CanvasAnimatedDrawEventArgs args,
            Action<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs, Vector2, Color> drawPoint,
            int startX,
            int startY,
            int endX,
            int endY,
            Color color)
        {
            for (var y = startY + InductionDistance / 2; y < endY; y += InductionDistance)
            {
                for (int x = startX + InductionDistance / 2; x < endX; x += InductionDistance)
                {
                    drawPoint(sender, args, new Vector2(x, y), Colors.Gray);
                }
            }
        }

        internal void DrawInductionCross(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, Vector2 point, Color color)
        {
            args.DrawingSession.DrawLine(point - new Vector2(0, 5), point + new Vector2(0, 5), color);
            args.DrawingSession.DrawLine(point - new Vector2(5, 0), point + new Vector2(5, 0), color);
        }

        internal void DrawInductionDot(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, Vector2 point, Color color)
        {
            args.DrawingSession.DrawCircle(point, 2, color);
        }
    }
}
