using Physics.HomongenousParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class RadiationMotionSetup : MotionSetupBase
    {
        public RadiationMotionSetup(float velocity, RadiationType type, string color)
        {
            Velocity = velocity;
            Type = type;
            Color = color;
        }

        public float Velocity { get; set; }

        public RadiationType Type { get; set; }
    }
}
