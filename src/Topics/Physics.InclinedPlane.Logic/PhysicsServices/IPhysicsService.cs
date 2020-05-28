using Physics.InclinedPlane.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Logic.PhysicsServices
{
    public interface IPhysicsService
    {
        float ComputeX(float time);
        float ComputeY(float time);
        float ComputeV(float time);
        float ComputeS(float time);
        float MaxT { get; }
        IMotionSetup Setup { get; set; }
    }
}
