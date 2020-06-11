using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Rendering;
using Physics.InclinedPlane.Services;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Extensions;
using Physics.Shared.UI.Rendering;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Physics.InclinedPlane.Rendering
{
    public class SimulationRenderer : IVariantRenderer
    {
        private readonly InclinedPlaneCanvasController _canvasController;
        private SimulationBounds _simulationBounds;
        private float _scalingRatio;
        private float _padding = 20;

        public SimulationRenderer(InclinedPlaneCanvasController _canvasController)
        {
            this._canvasController = _canvasController;
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var renderTarget = new CanvasRenderTarget(sender, (float)sender.Size.Width - _padding * 2, (float)sender.Size.Height - _padding * 2, 96.0f);
            using (var drawingSession = renderTarget.CreateDrawingSession())
            {
                drawingSession.Transform = Matrix3x2.CreateScale(new Vector2(1, -1)) * Matrix3x2.CreateTranslation(new Vector2(0, (float)renderTarget.Size.Height));
                drawingSession.Units = CanvasUnits.Dips;
                drawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
                DrawFloor(renderTarget, drawingSession);
                DrawInclinedPlane(renderTarget, drawingSession);
                DrawObject(renderTarget, drawingSession);
            }
            args.DrawingSession.DrawImage(renderTarget, new Vector2(_padding, _padding), renderTarget.Bounds );
        }

        private void DrawObject(CanvasRenderTarget target, CanvasDrawingSession session)
        {
            var t = (float)_canvasController.SimulationTime.TotalTime.TotalSeconds;
            var x = _canvasController.PhysicsService.CalculateX(t) * _scalingRatio;
            var y = _canvasController.PhysicsService.CalculateY(t) * _scalingRatio;
            
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_canvasController.Motion.Color);

            if (t <= _canvasController.PhysicsService.CalculateHorizontalStartTime() || !_canvasController.Motion.HasHorizontal)
            {
                var rectangle = CanvasGeometry.CreateRectangle(target, new Rect(0, 0, 24, 24)).Transform(Matrix3x2.CreateRotation(MathHelpers.DegreesToRadians(-_canvasController.Motion.InclinedAngle))).Transform(
                    Matrix3x2.CreateTranslation(x, y));
                //apply angle                
                session.FillGeometry(rectangle, color);
            }
            else
            {
                session.FillRectangle(
                    new Rect(x - 12, y, 24, 24),
                    color);
            }
        }

        public void Update(ICanvasAnimatedControl sender)
        {
            var physicsService = _canvasController.PhysicsService;
            var minX = 0;
            var maxX = physicsService.CalculateMaxX();
            var minY = 0;
            var maxY = physicsService.CalculateY(0);

            if (_canvasController.Motion.HorizontalLength == 0)
            {
                _padding = 60;
            }
            else
            {
                _padding = 20;
            }

            _simulationBounds = new SimulationBounds(minX, maxY, maxX, minY);
            _scalingRatio = _simulationBounds.Size.ScaleToFit(sender.Size.ReduceBy(_padding));
        }

        private void DrawInclinedPlane(CanvasRenderTarget sender, CanvasDrawingSession drawingSession)
        {
            var motion = _canvasController.Motion;
            var angleInRad = MathHelpers.DegreesToRadians(motion.InclinedAngle);
            var y = motion.InclinedLength * Math.Sin(angleInRad);
            var x = motion.InclinedLength * Math.Cos(angleInRad);
            drawingSession.DrawLine(
                new Vector2(0, (float)y * _scalingRatio),
                new Vector2((float)x * _scalingRatio, 0),
                Colors.Black);
        }

        private void DrawFloor(CanvasRenderTarget sender, CanvasDrawingSession drawingSession)
        {
            if (_canvasController.Motion.HasHorizontal) {
                drawingSession.DrawLine(
                    new Vector2(
                        _canvasController.PhysicsService.CalculateHorizontalStartX() * _scalingRatio,
                        1),
                    new Vector2(
                        (float)sender.Size.Width,
                        1),
                    Colors.Black);
            }
        }
    }
}
