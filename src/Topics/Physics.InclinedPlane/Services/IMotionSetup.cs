using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public interface IMotionSetup
    {
        float Elevation { get; }
        float Length { get; }
        float Gravity { get; }
        float DriftCoefficient { get; }

        string Color { get; }
    }
}
