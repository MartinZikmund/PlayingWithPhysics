using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class PerpendicularMotionSetup : MotionSetupBase
    {
        public PerpendicularMotionSetup(float velocity, float angle, float charge, PerpendicularInductionOrientation inductionOrientation, string color)
        {
            Velocity = velocity;
            Angle = angle;
            Charge = charge;
            InductionOrientation = inductionOrientation;
            Color = color;
        }

        public float Angle { get; set; }

        public float Charge { get; set; }

        public PerpendicularInductionOrientation InductionOrientation { get; set; }
    }
}
