using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Rendering
{
    public interface IVariantRenderer
    {
        void StartSimulation(IMotionSetup[] motions);

        void Update(ICanvasAnimatedControl sender);

        void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args);
    }
}
