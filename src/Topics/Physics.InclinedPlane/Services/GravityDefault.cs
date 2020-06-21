using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class GravityDefault
    {
        public GravityDefault(string name, float? value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public float? Value { get; }

        public bool HasValue => Value != null;

        public string FormattedValue => Value?.ToString("0.##") ?? string.Empty;
    }
}
