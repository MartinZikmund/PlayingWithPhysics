using Physics.Shared.Logic.Constants;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public static class MotionFactory
    {
        public static MotionInfo CreateFreeFall(Vector2 origin, float resistance, bool isBall, float mass, float area, float originSpeed, float elevationAngle, float gravity, float environmentDensity, float diameter, float shapeDensity, string color) =>
            new MotionInfo(MovementType.FreeFall, origin, resistance, isBall, mass, area, originSpeed, elevationAngle, gravity, environmentDensity, diameter, shapeDensity, color);

        public static MotionInfo CreateProjectileMotion(Vector2 origin, float resistance, bool isBall, float mass, float area, float originSpeed, float elevationAngle, float gravity, float environmentDensity, float diameter, float shapeDensity, string color) =>
			new MotionInfo(MovementType.ProjectileMotion, origin, resistance, isBall, mass, area, originSpeed, elevationAngle, gravity, environmentDensity, diameter, shapeDensity, color);
    }
}
