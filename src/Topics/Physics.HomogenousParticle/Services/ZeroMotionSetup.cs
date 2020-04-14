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
        public ZeroMotionSetup(float velocity, float charge, float inductionOrientation)
        {
            Velocity = velocity;
            Charge = charge;
            InductionOrientation = inductionOrientation;
        }
    }
}
