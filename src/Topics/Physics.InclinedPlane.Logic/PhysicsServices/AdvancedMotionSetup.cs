using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class AdvancedMotionSetup : MotionSetupBase
    {
        public AdvancedMotionSetup(float elevation, float angle, float mass, float driftCoefficient, float length, string color, float gravity = 9.81f)
            : base(elevation, angle, mass, driftCoefficient, length, color, gravity)
        {
        }
    }
}
