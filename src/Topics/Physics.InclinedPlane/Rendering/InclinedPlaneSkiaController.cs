using Physics.HomogenousParticle.Rendering;
using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.InclinedPlane.Services;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Rendering
{
    public class InclinedPlaneSkiaController : SkiaCanvasController
    {
        private ISkiaVariantRenderer _renderer;

        public InclinedPlaneSkiaController(SkiaCanvas canvasAnimatedControl) :
            base(canvasAnimatedControl)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            _renderer?.Dispose();
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

        public void SetVariantRenderer(ISkiaVariantRenderer renderer)
        {
            _renderer = renderer;
        }

        public override void Update(SkiaCanvas sender)
        {
            _renderer.Update(sender);
        }

        public override void Draw(SkiaCanvas sender, SKSurface args)
        {
            args.Canvas.Clear(new SKColor(255, 244, 244, 244));
            _renderer.Draw(sender, args);
        }
    }
}
