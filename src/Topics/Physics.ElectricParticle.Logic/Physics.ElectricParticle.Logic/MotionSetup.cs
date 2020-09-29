using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    public class MotionSetup : MotionSetupBase
    {
        public MotionSetup(VerticalLeftPlaneChargePolarity leftPlaneChargePolarity, string color)
        {
            LeftPlaneChargePolarity = leftPlaneChargePolarity;
            Color = color;
        }

        public VerticalLeftPlaneChargePolarity LeftPlaneChargePolarity { get; set; }
    }
}
