using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Toolkit.Uwp.Helpers;
using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class RadiationVariantRenderer : IVariantRenderer
    { 
        private RadiationMotionSetup[] _motions;
        private RadiationPhysicsService[] _physicsServices;
        private Vector2[] _positions;
        private HomogenousParticleCanvasController _controller;

        private const double TimeAdjustment = 2;
        private const double Padding = 20;

        public RadiationVariantRenderer(HomogenousParticleCanvasController controller)
        {
            _controller = controller;
        }

        public void StartSimulation(IMotionSetup[] motions)
        {
            if (motions is null)
            {
                throw new ArgumentNullException(nameof(motions));
            }

            _motions = motions.OfType<RadiationMotionSetup>().ToArray();
            _positions = new Vector2[_motions.Length];
            _physicsServices = _motions.Select(m => new RadiationPhysicsService(m)).ToArray();
        }

        public void Update(ICanvasAnimatedControl sender)
        {
            for (int i = 0; i < _physicsServices.Length; i++)
            {
                RadiationPhysicsService service = _physicsServices[i];
                var totalTime = _controller.SimulationTime.TotalTime.TotalSeconds - TimeAdjustment;
                _positions[i] = new Vector2(
                    (float)service.ComputeX(totalTime),
                    (float)service.ComputeY(totalTime));
            }
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var minDimension = Math.Min(sender.Size.Height, sender.Size.Height) - Padding;
            if (minDimension < 20) return;

            var topLeft = new Vector2(-2, 5);
            var adjustedTopLeft = AdjustPosition(topLeft, sender, minDimension);
            args.DrawingSession.FillRectangle(adjustedTopLeft.X, adjustedTopLeft.Y, 4 * (float)minDimension / 10, 5 * (float)minDimension / 10, Colors.Silver);

            for (int i = 0; i < _positions.Length; i++)
            {
                Vector2 position = (Vector2)_positions[i];
                var motion = _motions[i];
                var adjustedPosition = AdjustPosition(position, sender, minDimension);
                var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(motion.Color);
                _controller.DrawParticle(adjustedPosition, color, args);                
            }
        }

        private Vector2 AdjustPosition(Vector2 position, ICanvasAnimatedControl sender, double minDimension)
        {
            var y = sender.Size.Height - Padding - (position.Y + 2) * minDimension / 10;
            var x = position.X * minDimension / 10 + sender.Size.Width / 2;
            return new Vector2((float)x, (float)y);
        }
    }
}
