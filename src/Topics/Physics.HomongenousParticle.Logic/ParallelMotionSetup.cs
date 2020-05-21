using Physics.HomongenousParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class ParallelMotionSetup : MotionSetupBase
    {
        public ParallelMotionSetup(float velocity, ParallelVariantOrientation angle, float charge, float inductionOrientation, string color)
        {
            Velocity = velocity;
            Angle = angle;
            Charge = charge;
            InductionOrientation = inductionOrientation;
            Color = color;
        }

        public ParallelVariantOrientation Angle { get; set; }

        public float Charge { get; set; }

        public float InductionOrientation { get; set; }
    }
}
