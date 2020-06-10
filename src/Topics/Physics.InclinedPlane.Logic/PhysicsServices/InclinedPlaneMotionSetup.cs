using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class InclinedPlaneMotionSetup : IInclinedPlaneMotionSetup
    {
        public InclinedPlaneMotionSetup(
            float mass,
            float v0,
            float gravity,
            float inclinedLength, 
            float inclinedDriftCoefficient,
            float inclinedAngle, 
            float horizontalLength, 
            float horizontalDriftCoefficient, 
            string color)
        {
            Mass = mass;
            V0 = v0;
            InclinedAngle = inclinedAngle;
            InclinedDirftCoefficient = inclinedDriftCoefficient;
            InclinedLength = inclinedLength;
            Color = color ?? throw new ArgumentNullException(nameof(color));
            HorizontalLength = horizontalLength;
            HorizontalDriftCoefficient = horizontalDriftCoefficient;
            Gravity = gravity;
        }

        public float Mass { get; set; }

        public float V0 { get; set; }

        public bool HasHorizontal => HorizontalLength > 0;

        public float InclinedAngle { get; set; }

        public float InclinedLength { get; set; }

        public float Gravity { get; set; }

        public float InclinedDirftCoefficient { get; set; }

        public float HorizontalLength { get; set; }

        public float HorizontalDriftCoefficient { get; set; }

        public string Color { get; set; }
    }
}
