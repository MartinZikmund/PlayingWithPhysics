using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.Shared.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Rendering
{
    public class HomogenousParticleCanvasController : BaseCanvasController, IDisposable
    {
        public HomogenousParticleCanvasController(CanvasAnimatedControl canvasAnimatedControl) : base(canvasAnimatedControl)
        {
        }

        public override Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            throw new NotImplementedException();
        }

        public override void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            throw new NotImplementedException();
        }

        public override void Update(ICanvasAnimatedControl sender)
        {
            throw new NotImplementedException();
        }
    }
}
