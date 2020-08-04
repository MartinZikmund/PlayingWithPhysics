using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public class GravityCoefficient
    {

        public string Name { get; set; }
        public float? Value { get; set; }

        public GravityCoefficient(string name, float value)
        {
            Name = name;
            Value = value;
        }
        public string FormattedValue => Value?.ToString("0.#") ?? string.Empty;
    }
}
