using Microsoft.Graphics.Canvas;
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
                drawingSession.Units = CanvasUnits.Dips;
                drawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
                DrawFloor(renderTarget, drawingSession);
                DrawInclinedPlane(renderTarget, drawingSession);
                var x = _canvasController.PhysicsService.ComputeX((float)_canvasController.SimulationTime.TotalTime.TotalSeconds);
                var y = FlipY(_canvasController.PhysicsService.ComputeY((float)_canvasController.SimulationTime.TotalTime.TotalSeconds) * _scalingRatio, renderTarget);
                drawingSession.FillCircle(
                    new Vector2(x * _scalingRatio,
                     y),
                    20, Colors.Red);
            }
            args.DrawingSession.DrawImage(renderTarget, new Vector2(_padding, _padding), renderTarget.Bounds );
        }

        public void Update(ICanvasAnimatedControl sender)
        {
            var physicsService = _canvasController.PhysicsService;
            var minX = 0;
            var maxX = physicsService.MaxX;
            var minY = 0;
            var maxY = physicsService.ComputeY(0);

            if (_canvasController.Motion.FinishLength == 0)
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
            var angleInRad = MathHelpers.DegreesToRadians(motion.Angle);
            var y = motion.Length * Math.Sin(angleInRad);
            var x = motion.Length * Math.Cos(angleInRad);
            drawingSession.DrawLine(
                new Vector2(0, FlipY((float)y * _scalingRatio, sender)),
                new Vector2((float)x * _scalingRatio, FlipY(0, sender)),
                Colors.Black);
        }

        private void DrawFloor(CanvasRenderTarget sender, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawLine(
                new Vector2(
                    0,
                    FlipY(0, sender)),
                new Vector2(
                    (float)sender.Size.Width,
                    FlipY(0, sender)),
                Colors.Black);
        }

        private float FlipY(float y, CanvasRenderTarget sender) => (float)sender.Size.Height - y;
    }
}
