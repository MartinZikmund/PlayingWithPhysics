using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    public abstract class MotionSetupBase : IMotionSetup
    {
        public MotionSetupBase() { }

        public MotionSetupBase(string color)
        {
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public string Color { get; set; }
    }
}