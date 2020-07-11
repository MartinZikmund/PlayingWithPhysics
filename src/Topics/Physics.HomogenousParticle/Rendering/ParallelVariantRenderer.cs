using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic;
using Physics.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Rendering
{
    public class ParallelVariantRenderer : IVariantRenderer
    {
        private ParallelMotionSetup _motion;
        private readonly HomogenousParticleCanvasController _controller;

        public ParallelVariantRenderer(HomogenousParticleCanvasController controller)
        {
            _controller = controller;
        }

        public void StartSimulation(IMotionSetup[] motions)
        {
            if (motions?.Length != 1)
            {
                throw new ArgumentException("First variant supports only one motion", nameof(motions));
            }
            _motion = motions[0] as ParallelMotionSetup;
            _lastPosition = null;
        }

        private Vector2? _lastPosition = null;

        public void Update(ICanvasAnimatedControl sender)
        {
            var unit = sender.Size.Width / 1000;
            var actualVelocity = _motion.Velocity * 20;
            if (_lastPosition == null)
            {
                _lastPosition = new Vector2((float)sender.Size.Width / 2f, (float)sender.Size.Height / 2f);
            }
            else
            {
                var direction = new Vector2(
                    (float)(unit * actualVelocity * _controller.SimulationTime.ElapsedTime.TotalSeconds * Math.Cos(MathHelpers.DegreesToRadians(_motion.InductionOrientation))),
                    (float)(unit * actualVelocity * _controller.SimulationTime.ElapsedTime.TotalSeconds * Math.Sin(-MathHelpers.DegreesToRadians(_motion.InductionOrientation)))
                    );
                if (_motion.Angle == ParallelVariantOrientation.Opposite)
                {
                    direction *= -1;
                }
                _lastPosition += direction;
            }
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));

            if (_motion != null)
            {
                var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motion.Color);
                _controller.DrawInductionArrows(_motion.InductionOrientation, color, args);

                _controller.DrawParticle(_lastPosition.Value, color, args);
            }
        }
    }
}
