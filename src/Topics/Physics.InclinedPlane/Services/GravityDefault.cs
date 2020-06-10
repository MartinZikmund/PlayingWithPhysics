using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Services
{
    public class GravityDefault
    {
        public GravityDefault(string name, float value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public float Value { get; set; }
    }
}
