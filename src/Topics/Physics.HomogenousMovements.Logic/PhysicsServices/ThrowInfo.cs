using Physics.HomogenousMovement.PhysicsServices;
using Physics.Shared.Logic.Constants;
using System;
using System.Numerics;

namespace Physics.HomogenousMovement.Logic.PhysicsServices
{
    public class ThrowInfo
    {
        public ThrowInfo(MovementType movementType, Vector2 origin, float mass, float v0, float angle, string color, float g = GravityConstants.Earth)
        {
            if (mass == 0f)
                throw new ArgumentException("Hmotnost musí být větší než 1 kg.");
            Type = movementType;
            Mass = mass;
            Origin = origin;
            V0 = v0;
            Angle = angle;
            Color = color;
            G = g;
        }

        public ThrowInfo()
        {
        }

        public MovementType Type { get; set; }
        public Vector2 Origin { get; set; }
        public float G { get; set; }
        public float V0 { get; set; }
        public float Mass { get; set; }
        public float Angle { get; set; }
        public string Color { get; set; }
        public string Label { get; set; }
    }
}