using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public interface IPhysicsService
    {
        float ComputeX(float timeMoment);
        float ComputeY(float timeMoment);
        float ComputeV(float timeMoment);
        float ComputeVX(float timeMoment);
        float ComputeVY(float timeMoment);
        float ComputeEP(float timeMoment);
        float ComputeEK(float timeMoment);
        float ComputeEPEK(float timeMoment);
        float MaxX { get; }
        float MaxT { get; }
        float MaxY { get; }
        MotionInfo MotionInfo { get; }

        //TrajectoryData CreateTrajectoryData();
    }
}
