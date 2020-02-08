using System.Numerics;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.Shared.Logic.Constants;

namespace Physics.HomogenousMovement.PhysicsServices
{
    public static class MotionFactory
    {
        public static MotionInfo CreateFreeFall(Vector2 origin, float mass, float timeFrame,
             string color, float g = GravityConstants.Earth) =>
            new MotionInfo(MovementType.FreeFall, origin, mass, 0, 0, color, g);

        public static MotionInfo CreateUpwardMotion(Vector2 origin, float mass, float timeFrame, float v0, 
            string color, float g = GravityConstants.Earth) => new MotionInfo(MovementType.UpwardMotion, origin, mass, v0, 90, color, g);

        public static MotionInfo CreateHorizontalMotion(Vector2 origin, float mass, float timeFrame, float v0,
             string color, float g = GravityConstants.Earth) => new MotionInfo(MovementType.ForwardMotion, origin, mass, v0, 0, color, g);

        public static MotionInfo CreateProjectileMotion(Vector2 origin, float mass, float timeFrame, float v0,
             string color, float angle, float g = GravityConstants.Earth) => new MotionInfo(MovementType.ProjectileMotion, origin, mass, v0, angle, color, g);
    }
}