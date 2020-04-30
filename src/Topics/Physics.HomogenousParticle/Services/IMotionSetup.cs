using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public interface IMotionSetup
    {
        float Velocity { get; }

        string Color { get; }
    }
}
