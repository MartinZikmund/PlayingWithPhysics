using Physics.Shared.Logic.Constants;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public enum MovementType
    {
        FreeFall,
        VerticalMotion,
        HorizontalMotion,
        ProjectileMotion
    }

    public class MotionInfo
    {
        public MotionInfo(MovementType movementType, Vector2 origin, float mass, float v0, float angle, string color, float g = GravityConstants.Earth)
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

        public MotionInfo()
        {
        }

        public MotionInfo Clone()
        {
            return new MotionInfo(this.Type, Origin, Mass, V0, Angle, Color, G)
            {
                Label = Label,
            };
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
