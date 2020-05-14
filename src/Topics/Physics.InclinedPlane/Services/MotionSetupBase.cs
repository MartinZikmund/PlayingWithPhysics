using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public abstract class MotionSetupBase : IMotionSetup
    {
        public MotionSetupBase()
        {

        }

        public MotionSetupBase(float elevation, float length, float driftCoefficient, string color, float gravity)
        {
            Elevation = elevation;
            Length = length;
            DriftCoefficient = driftCoefficient;
            Gravity = gravity;
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public float Elevation { get; set; }
        public float Length { get; set; }
        public float Gravity { get; set; }
        public float DriftCoefficient { get; set; }

        public string Color { get; set; }
    }
}
