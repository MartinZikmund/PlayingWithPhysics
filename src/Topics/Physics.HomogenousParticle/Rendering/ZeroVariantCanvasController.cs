using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Services;
using System;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class ZeroVariantCanvasController : HomogenousParticleCanvasControllerBase
    {
        private ZeroMotionSetup _motion = null;

        public ZeroVariantCanvasController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public override void StartSimulation(IMotionSetup[] motions)
        {
            if (motions?.Length != 1)
            {
                throw new ArgumentException("First variant supports only one motion", nameof(motions));
            }
            _motion = motions[0] as ZeroMotionSetup;            
        }

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            return Task.CompletedTask;
        }

        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));

            DrawInductionArrows(_motion.InductionOrientation, Colors.Red, args);   
        }
    }
}
