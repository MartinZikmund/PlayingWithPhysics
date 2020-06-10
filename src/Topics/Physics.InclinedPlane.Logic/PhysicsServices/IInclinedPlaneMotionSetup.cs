using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public interface IInclinedPlaneMotionSetup
    {
        float Mass { get; }

        float V0 { get; }

        bool HasHorizontal { get; }

        float InclinedAngle { get; }

        float InclinedLength { get; }

        float Gravity { get; }

        float InclinedDirftCoefficient { get; }

        float HorizontalLength { get; }

        float HorizontalDriftCoefficient { get; }

        string Color { get; }
    }
}