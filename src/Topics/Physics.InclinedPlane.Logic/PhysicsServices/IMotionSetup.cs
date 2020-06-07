using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public interface IMotionSetup
    {
        float Angle { get; }
        float Mass { get; }
        float Length { get; }
        float Gravity { get; }
        float DriftCoefficient { get; }
        float FinishLength { get; set; }
        float FinishDriftCoefficient { get; set; }

        string Color { get; }
    }
}
