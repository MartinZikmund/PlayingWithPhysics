using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Services;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class PerpendicularVariantRenderer : IVariantRenderer
    {
        private const int InductionDistance = 40;

        private PerpendicularMotionSetup[] _motions;
        private readonly HomogenousParticleCanvasController _controller;

        public PerpendicularVariantRenderer(HomogenousParticleCanvasController controller)
        {
            _controller = controller;
        }

        public void StartSimulation(IMotionSetup[] motions)
        {
            if (motions?.Length != 1)
            {
                throw new ArgumentException("First variant supports only one motion", nameof(motions));
            }
            _motions = motions.OfType<PerpendicularMotionSetup>().ToArray();
            _currentMotionPositions = new Vector2[_motions.Length];
            _lastPosition = null;
        }

        private Vector2? _lastPosition = null;

        private Vector2[] _currentMotionPositions = null;

        public void Update(ICanvasAnimatedControl sender)
        {
            if (_motions != null)
            {
                var dimension = Math.Min(sender.Size.Width, sender.Size.Height);
                var radius = dimension / 6;

                for (int i = 0; i < _motions.Length; i++)
                {
                    _currentMotionPositions[i] = CalculateMotionPosition(_motions[i], (float)radius);
                }
            }
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
            var drawingOffset = new Vector2((float)sender.Size.Width / 2f, (float)sender.Size.Height / 2f);
            if (_motions != null)
            {
                DrawInductionDirection(sender, args);
                for (int i = 0; i < _motions.Length; i++)
                {
                    var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motions[i].Color);
                    _controller.DrawParticle(_currentMotionPositions[i] + drawingOffset, color, args);
                }
            }
        }

        private void DrawInductionDirection(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if (_motions[0].InductionOrientation == PerpendicularInductionOrientation.FromPaper)
            {
                DrawInductionGrid(sender, args, DrawInductionCross);
            }
            else
            {
                DrawInductionGrid(sender, args, DrawInductionDot);
            }
        }

        private void DrawInductionGrid(
            ICanvasAnimatedControl sender, 
            CanvasAnimatedDrawEventArgs args, 
            Action<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs, Vector2, Color> drawPoint)
        {
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motions[0].Color);
            for (var y = InductionDistance / 2; y < sender.Size.Height; y += InductionDistance)
            {
                for (int x = InductionDistance / 2; x < sender.Size.Width; x += InductionDistance)
                {
                    drawPoint(sender, args, new Vector2(x, y), color);
                }
            }
        }

        private void DrawInductionCross(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, Vector2 point, Color color)
        {
            args.DrawingSession.DrawLine(point - new Vector2(0, 5), point + new Vector2(0, 5), color);
            args.DrawingSession.DrawLine(point - new Vector2(5, 0), point + new Vector2(5, 0), color);
        }

        private void DrawInductionDot(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, Vector2 point, Color color)
        {
            args.DrawingSession.DrawCircle(point, 2, color);
        }

        private Vector2 CalculateMotionPosition(PerpendicularMotionSetup motionSetup, float radius)
        {
            var t = _controller.SimulationTime.TotalTime.TotalSeconds;
            var angle = MathHelpers.DegreesToRadians(motionSetup.Angle);
            var x =
                radius *
                Math.Cos(-(Math.PI / 2) * t - (3 * Math.PI / 2) + angle) +
                radius *
                Math.Cos(angle - Math.PI / 2);
            var y =
                radius *
                Math.Sin(-(Math.PI / 2) * t - (3 * Math.PI / 2) + angle) +
                radius *
                Math.Cos(angle - Math.PI / 2);
            return new Vector2((float)x, (float)y);
        }
    }
}
