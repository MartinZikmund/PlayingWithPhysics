using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.DragMovement.Logic.PhysicsServices
{
    public class EnvironmentDensity
    {
        public string Name { get; set; }
        public float? Value { get; set; }

        public EnvironmentDensity(string name, float value)
        {
            Name = name;
            Value = value;
        }
        public string FormattedValue => Value?.ToString("0.##") ?? string.Empty;
    }
}
