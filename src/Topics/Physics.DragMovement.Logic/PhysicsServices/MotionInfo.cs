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
        ProjectileMotion
    }

    public class MotionInfo
    {

        public MotionInfo()
        {
        }
        public MotionInfo(MovementType movementType, Vector2 origin, float resistance, float mass, float area, float originSpeed, float elevationAngle, float gravity, float environmentDensity, float diameter, float shapeDensity, string color)
        {
            if (mass == 0f)
                throw new ArgumentException("Hmotnost musí být větší než 1 kg.");
            Type = movementType;
            Origin = origin;
            Resistance = resistance;
            _mass = mass;
            _area = area;
            OriginSpeed = originSpeed;
            ElevationAngle = elevationAngle;
            G = gravity;
            EnvironmentDensity = environmentDensity;
            Diameter = diameter;
            ShapeDensity = shapeDensity;
            Color = color;
        }

        public MotionInfo Clone()
        {
            return new MotionInfo(this.Type, Origin, Resistance, Mass, Area, OriginSpeed, ElevationAngle, G, EnvironmentDensity, Diameter, ShapeDensity, Color)
            {
                Label = Label
            };
        }

        public MovementType Type { get; set; }
        public Vector2 Origin { get; set; }
        public float Resistance { get; set; }
        public float Mass
        {
            get
            {
                if (_mass >= 0)
                {
                    return _mass;
                }
                else
                {
                    return ShapeDensity * (4 / 3 * (float)Math.PI * (float)Math.Pow(Diameter, 3));
                }
            }
        }
        private float _mass = -1;
        public float Area
        {
            get
            {
                if (_area >= 0)
                {
                    return _area;
                }
                else
                {
                    return (float)Math.PI * (float)Math.Pow(Diameter, 2);
                }
            }
        }
        
        public float _area = -1;
        public float OriginSpeed { get; set; }
        public float ElevationAngle { get; set; }
        public float G { get; set; }
        public float EnvironmentDensity { get; set; }
        public float Diameter { get; set; }
        public float ShapeDensity { get; set; }
        public string Color { get; set; }
        public string Label { get; set; }
    }
}
