using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class BasicMotionSetup : MotionSetupBase
    {
        public BasicMotionSetup()
        {
        }

        public BasicMotionSetup(float elevation, float length, float driftCoefficient, string color)
        {
            Elevation = elevation;
            Length = length;
            DriftCoefficient = driftCoefficient;
            Color = color;
            Gravity = 9.81f;
        }
    }
}
