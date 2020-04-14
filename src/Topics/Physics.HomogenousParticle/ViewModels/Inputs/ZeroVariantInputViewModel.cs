using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class ZeroVariantInputViewModel : IVariantInputViewModel
    {
        public IMotionSetup CreateMotionSetup()
        {
            return new ZeroMotionSetup(Velocity, Charge, InductionOrientation);
        }

        public float Charge { get; set; }
        public float InductionOrientation { get; set; }
        public float Velocity { get; set; }
    }
}
