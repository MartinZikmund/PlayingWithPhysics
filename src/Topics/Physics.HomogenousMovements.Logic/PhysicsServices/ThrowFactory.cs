using System.Numerics;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.Shared.Logic.Constants;

namespace Physics.HomogenousMovement.PhysicsServices
{
    public static class MotionFactory
    {
        public static MotionInfo CreateFreeFall(Vector2 origin, float mass, float timeFrame,
             string color, float g = PhysicsConstants.EarthGravity) =>
            new MotionInfo(MovementType.FreeFall, origin, mass, 0, 0, color, g);

        public static MotionInfo CreateUpwardMotion(Vector2 origin, float mass, float timeFrame, float v0, 
            string color, float g = PhysicsConstants.EarthGravity) => new MotionInfo(MovementType.VerticalMotion, origin, mass, v0, 90, color, g);

        public static MotionInfo CreateHorizontalMotion(Vector2 origin, float mass, float timeFrame, float v0,
             string color, float g = PhysicsConstants.EarthGravity) => new MotionInfo(MovementType.HorizontalMotion, origin, mass, v0, 0, color, g);

        public static MotionInfo CreateProjectileMotion(Vector2 origin, float mass, float timeFrame, float v0,
             string color, float angle, float g = PhysicsConstants.EarthGravity) => new MotionInfo(MovementType.ProjectileMotion, origin, mass, v0, angle, color, g);
    }
}