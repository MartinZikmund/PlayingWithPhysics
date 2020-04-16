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
            return new ZeroMotionSetup(Charge, InductionOrientation);
        }

        public float Charge { get; set; } // -3<=q<=3
        public float InductionOrientation { get; set; } // 0<=B<=360
    }
}
