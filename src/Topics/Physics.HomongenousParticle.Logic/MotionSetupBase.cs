using Microsoft.Toolkit.Uwp.Helpers;
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

        public MotionSetupBase(float velocity, string color)
        {           
            Velocity = velocity;
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }

        public float Velocity { get; set; }

        public string Color { get; set; }
    }
}
