using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    public class EnvironmentSetting
    {
        public string Name { get; set; }

        public float Value { get; set; }

        public EnvironmentSetting(string name, float value)
        {
            Name = name;
            Value = value;
        }
        public string FormattedValue => Value.ToString("0.##") ?? string.Empty;
    }
}
