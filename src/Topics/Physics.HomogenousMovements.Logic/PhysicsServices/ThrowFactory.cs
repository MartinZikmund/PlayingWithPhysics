using System.Numerics;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Physics.Shared.Logic.Constants;

namespace Physics.HomogenousMovement.PhysicsServices
{
    public static class ThrowFactory
    {
        public static ThrowInfo CreateFreeFall(Vector2 origin, float mass, float timeFrame,
             string color, float g = GravityConstants.Earth) =>
            new ThrowInfo(MovementType.FreeFall, origin, mass, 0, 0, color, g);

        public static ThrowInfo CreateUpwardThrow(Vector2 origin, float mass, float timeFrame, float v0, 
            string color, float g = GravityConstants.Earth) => new ThrowInfo(MovementType.UpwardThrow, origin, mass, v0, 90, color, g);

        public static ThrowInfo CreateHorizontalThrow(Vector2 origin, float mass, float timeFrame, float v0,
             string color, float g = GravityConstants.Earth) => new ThrowInfo(MovementType.ForwardThrow, origin, mass, v0, 0, color, g);

        public static ThrowInfo CreateProjectileMotion(Vector2 origin, float mass, float timeFrame, float v0,
             string color, float angle, float g = GravityConstants.Earth) => new ThrowInfo(MovementType.ProjectileMotion, origin, mass, v0, angle, color, g);
    }
}