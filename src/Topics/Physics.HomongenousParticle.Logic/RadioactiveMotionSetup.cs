﻿using Physics.HomongenousParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class RadioactiveMotionSetup : MotionSetupBase
    {
        public RadioactiveMotionSetup(float velocity, RadiationType type, string color)
        {
            Velocity = velocity;
            Type = type;
            Color = color;
        }

        public RadiationType Type { get; set; }
    }
}