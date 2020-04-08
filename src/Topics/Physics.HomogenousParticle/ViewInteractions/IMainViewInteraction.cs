using Physics.HomogenousParticle.Rendering;
using Physics.Shared.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewInteractions
{
    public interface IMainViewInteraction
    {
        HomogenousParticleCanvasController Initialize(DifficultyOption difficulty);
    }
}
