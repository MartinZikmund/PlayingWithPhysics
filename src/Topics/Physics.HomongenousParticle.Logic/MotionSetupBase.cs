using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public abstract class MotionSetupBase : IMotionSetup
    {
        public MotionSetupBase()
        {

        }

        public MotionSetupBase(string label, string color)
        {
            Label = label;
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public string Color { get; set; }

        public string Label { get; set; }
    }
}
