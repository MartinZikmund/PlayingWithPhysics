﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Services
{
    public class ParallelMotionSetup : MotionSetupBase
    {
        public ParallelMotionSetup(float velocity, float angle, float charge, float inductionOrientation)
        {
            Velocity = velocity;
            Angle = angle;
            Charge = charge;
            InductionOrientation = inductionOrientation;
        }

        public float Angle { get; set; }
        public float Charge { get; set; }
        public float InductionOrientation { get; set; }
    }
}