using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public abstract class MotionSetupBase : IMotionSetup
    {
        public float Velocity { get; set; }
    }
}
