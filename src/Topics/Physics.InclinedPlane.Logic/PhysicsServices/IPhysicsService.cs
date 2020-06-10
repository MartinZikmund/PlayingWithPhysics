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
        float CalculateX(float time);
        float CalculateY(float time);
        float CalculateV(float time);
        float CalculateS(float time);
        float CalculateEv(float time);
        float CalculateMaxT();

        IInclinedPlaneMotionSetup Setup { get; set; }
    }
}
