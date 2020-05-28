using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class BasicMotionSetup : MotionSetupBase
    {
        public BasicMotionSetup(float elevation, float angle, float mass, float driftCoefficient, float length, string color)
            : base(elevation, angle, mass, driftCoefficient, length, color, 9.81f)
        {
        }
    }
}
