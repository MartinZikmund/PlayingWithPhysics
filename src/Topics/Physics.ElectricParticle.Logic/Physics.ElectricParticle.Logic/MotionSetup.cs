using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    public class MotionSetup : MotionSetupBase
    {
        public MotionSetup(float chargeMultiple, float massMultiple, float velocityMultiple, float induction, PerpendicularInductionOrientation inductionOrientation, string color)
        {
            ChargeMultiple = chargeMultiple;
            MassMultiple = massMultiple;
            VelocityMultiple = velocityMultiple;
            Induction = induction;
            //InductionOrientation = inductionOrientation;
            Color = color;
        }

        public float ChargeMultiple { get; set; }

        public float MassMultiple { get; set; }

        public float VelocityMultiple { get; set; }

        public float Induction { get; set; }

        //public PerpendicularInductionOrientation InductionOrientation { get; set; }
    }
}
