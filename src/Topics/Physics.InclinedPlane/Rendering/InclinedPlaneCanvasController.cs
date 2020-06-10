using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Rendering;
using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.InclinedPlane.Services;
using Physics.Shared.UI.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Rendering
{
    public class InclinedPlaneCanvasController : BaseCanvasController
    {
        private IVariantRenderer _renderer;

        public InclinedPlaneCanvasController(CanvasAnimatedControl canvasAnimatedControl) :
            base(canvasAnimatedControl)
        {
        }

        public IInclinedPlaneMotionSetup Motion { get; private set; }

        public PhysicsService PhysicsService { get; private set; }

        public void StartSimulation(IInclinedPlaneMotionSetup motion)
        {
            if (motion is null)
            {
                throw new ArgumentNullException(nameof(motion));
            }

            Motion = motion;
            PhysicsService = new PhysicsService(motion);
            SimulationTime.Restart();
        }

        public void SetVariantRenderer(IVariantRenderer renderer)
        {
            _renderer = renderer;
        }

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            return Task.CompletedTask;
        }


        public override void Update(ICanvasAnimatedControl sender)
        {
            _renderer.Update(sender);
        }

        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = Microsoft.Graphics.Canvas.CanvasAntialiasing.Antialiased;
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(255, 244, 244, 244));
            _renderer.Draw(sender, args);
        }
    }
}
