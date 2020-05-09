using Physics.HomogenousParticle.Rendering;
using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewInteractions
{
    public interface IMainViewInteraction
    {
        HomogenousParticleCanvasController PrepareController(VelocityVariant variant);
    }
}
