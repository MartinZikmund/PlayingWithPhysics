using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class AdvancedMotionSetup : MotionSetupBase
    {
        public AdvancedMotionSetup(float angle, float mass, float driftCoefficient, float length, string color, float finishLength, float finishDriftCoefficient, float gravity = 9.81f)
            : base(angle, mass, driftCoefficient, length, color, finishLength, finishDriftCoefficient, gravity)
        {
        }
    }
}
