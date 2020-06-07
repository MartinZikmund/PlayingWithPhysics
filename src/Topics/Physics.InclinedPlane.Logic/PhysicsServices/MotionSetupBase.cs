using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public abstract class MotionSetupBase : IMotionSetup
    {
        public MotionSetupBase(float angle, float mass, float driftCoefficient, float length, string color, float finishLength, float finishDriftCoefficient, float gravity)
        {
            Angle = angle;
            Mass = mass;
            DriftCoefficient = driftCoefficient;
            Length = length;
            Color = color ?? throw new ArgumentNullException(nameof(color));
            FinishLength = finishLength;
            FinishDriftCoefficient = finishDriftCoefficient;
            Gravity = gravity;
        }

        public float Angle { get; set; }
        public float Mass { get; set; }
        public float Length { get; set; }
        public float Gravity { get; set; }
        public float DriftCoefficient { get; set; }
        public float FinishLength { get; set; }
        public float FinishDriftCoefficient { get; set; }
        public string Color { get; set; }
    }
}
