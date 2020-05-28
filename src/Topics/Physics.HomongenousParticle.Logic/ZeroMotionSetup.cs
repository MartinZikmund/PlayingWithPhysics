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
        public ZeroMotionSetup()
        {

        }

        public ZeroMotionSetup(float charge, float inductionOrientation, string color)
        {
            Charge = charge;
            InductionOrientation = inductionOrientation;
            Color = color;
        }

        public float Charge { get; set; }

        public float InductionOrientation { get; set; }
    }
}
