using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public interface IInclinedPlaneMotionSetup
    {
        float InclinedAngle { get; }

        float InclinedLength { get; }
                
        float InclinedDirftCoefficient { get; }
        
        float HorizontalLength { get; set; }
        
        float HorizontalDriftCoefficient { get; set; }

        float Gravity { get; }

        float Mass { get; }

        string Color { get; }
    }
}
