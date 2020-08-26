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
        float CalculateEk(float time);
        float CalculateEp(float time);
        float CalculateEm(float time);
        float CalculateE();
        float CalculateU(float time);
        float CalculateMaxT();

        float CalculateHorizontalStartX();

        float CalculateRemainingInclinedLength(float time);

        float CalculateRemainingInclinedX(float time);

        float CalculateTotalWidth();

        float CalculateFt(float time);

        float CalculateFp(float time);

        IInclinedPlaneMotionSetup Setup { get; set; }
    }
}
