﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public interface IMotionSetup
    {
        float Velocity { get; }
        float Charge { get; }
        float InductionOrientation { get; }
    }
}
