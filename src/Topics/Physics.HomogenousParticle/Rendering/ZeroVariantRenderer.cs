using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Services;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class ZeroVariantRenderer : IVariantRenderer
    {
        private ZeroMotionSetup _motion = null;
        private readonly HomogenousParticleCanvasController _controller;

        public ZeroVariantRenderer(HomogenousParticleCanvasController controller)
        {
            _controller = controller;
        }

        public void StartSimulation(IMotionSetup[] motions)
        {
            if (motions?.Length != 1)
            {
                throw new ArgumentException("First variant supports only one motion", nameof(motions));
            }
            _motion = motions[0] as ZeroMotionSetup;
        }

        public void Update(ICanvasAnimatedControl sender)
        {
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));

            if (_motion != null)
            {
                var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(_motion.Color);
                _controller.DrawInductionArrows(_motion.InductionOrientation, color, args);

                _controller.DrawParticle(new Vector2((float)sender.Size.Width / 2f, (float)sender.Size.Height / 2f), color, args);

                Microsoft.Graphics.Canvas.Text.CanvasTextFormat format = new Microsoft.Graphics.Canvas.Text.CanvasTextFormat()
                {
                    FontSize = 14,
                    HorizontalAlignment = Microsoft.Graphics.Canvas.Text.CanvasHorizontalAlignment.Center,
                };
            }
        }        
    }
}
