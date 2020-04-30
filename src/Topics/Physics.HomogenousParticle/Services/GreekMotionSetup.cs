using Physics.HomogenousParticle.ViewModels.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class GreekMotionSetup : MotionSetupBase
    {
        public GreekMotionSetup(float velocity, RadiationType type, string color)
        {
            Velocity = velocity;
            Type = type;
            Color = color;
        }

        public RadiationType Type { get; set; }
    }
}
