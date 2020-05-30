using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.State
{
    public static class VariantStateViewModelFactory
    {
        public static IVariantStateViewModel Create(IMotionSetup setup) =>
            setup switch
            {
                ZeroMotionSetup _ => new ZeroVariantStateViewModel(setup),
                ParallelMotionSetup _ => new ParallelVariantStateViewModel(setup),
                PerpendicularMotionSetup _ => new PerpendicularVariantStateViewModel(setup),
                RadiationMotionSetup _ => new RadiationVariantStateViewModel(setup),
                _ => throw new NotImplementedException(),               
            };
    }
}
