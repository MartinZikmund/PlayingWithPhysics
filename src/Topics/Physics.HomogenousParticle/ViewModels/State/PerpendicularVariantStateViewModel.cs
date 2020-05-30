using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.State
{
    public class PerpendicularVariantStateViewModel : VariantStateViewModelBase
    {
        public PerpendicularVariantStateViewModel(IMotionSetup motionSetup) : base(motionSetup)
        {
        }
    }
}
