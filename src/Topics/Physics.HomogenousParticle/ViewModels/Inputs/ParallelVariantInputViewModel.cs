using Physics.HomogenousParticle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels.Inputs
{
    public class ParallelVariantInputViewModel : IVariantInputViewModel
    {
        public IMotionSetup CreateMotionSetup()
        {
            return new ParallelMotionSetup(Velocity, Angle, Charge, InductionOrientation);
        }

        public float Velocity { get; set; } // 10^n: 2<=n<=6
        public float Angle { get; set; } // po směru: 1, proti směru: -1
        public float Charge { get; set; } // -3<=q<=3
        public float InductionOrientation { get; set; } // 0 <= B <= 360
        public string Label { get; set; }
    }
}
