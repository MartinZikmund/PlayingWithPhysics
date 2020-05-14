using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class AdvancedMotionSetup : MotionSetupBase
    {
        public AdvancedMotionSetup()
        {

        }

        public AdvancedMotionSetup(float elevation, float length, float driftCoefficient, string color, float gravity = 9.81f)
        {
            Elevation = elevation;
            Length = length;
            DriftCoefficient = driftCoefficient;
            Color = color;
            Gravity = Gravity;
        }
    }
}
