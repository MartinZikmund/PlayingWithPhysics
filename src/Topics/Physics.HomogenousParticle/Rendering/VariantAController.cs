using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.HomogenousParticle.Rendering
{
    public class VariantAController : HomogenousParticleCanvasControllerBase
    {
        public VariantAController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            return Task.CompletedTask;
        }

        private int i = 0;
        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));

            DrawInductionArrows(i++, Colors.Red, args);   
        }
    }
}
