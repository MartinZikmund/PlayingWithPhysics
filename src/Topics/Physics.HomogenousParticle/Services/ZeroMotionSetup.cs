using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class ZeroMotionSetup : MotionSetupBase
    {
        public ZeroMotionSetup(float charge, float inductionOrientation)
        {
            Charge = charge;
            InductionOrientation = inductionOrientation;
        }

        public float Charge { get; set; }
        public float InductionOrientation { get; set; }
    }
}
