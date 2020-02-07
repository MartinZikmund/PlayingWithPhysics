using System.Numerics;
using Physics.HomogenousMovement.Logic.PhysicsServices;

namespace Physics.HomogenousMovement.PhysicsServices
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
        ThrowInfo ThrowInfo { get; }

        TrajectoryData CreateTrajectoryData();
    }
}
