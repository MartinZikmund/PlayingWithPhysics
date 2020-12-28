using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.ElectricParticle.Logic
{
    public class MotionSetup : MotionSetupBase
    {
        public MotionSetup(InputVariant variant, PrimaryPlaneChargePolarity primaryPlaneChargePolarity,
                           float primaryVoltage,
                           float primaryPlaneDistance,
                           SecondaryPlaneChargePolarity secondaryPlaneChargePolarity,
                           float secondaryVoltage,
                           float secondaryPlaneDistance,
                           ChargePolarity chargePolarity,
                           float chargeBase,
                           float chargePower,
                           float massBase,
                           float massPower,
						   float velocity,
						   float deviation,
                           VelocityDirection velocityDirection,
                           EnvironmentSetting environmentSetting,
                           string color)
        {
			Variant = variant;
            PrimaryPlaneChargePolarity = primaryPlaneChargePolarity;
            PrimaryVoltage = primaryVoltage;
            PrimaryPlaneDistance = primaryPlaneDistance;
            SecondaryPlaneChargePolarity = secondaryPlaneChargePolarity;
            SecondaryVoltage = secondaryVoltage;
            SecondaryPlaneDistance = secondaryPlaneDistance;
            ChargePolarity = chargePolarity;
            ChargeBase = chargeBase;
            ChargePower = chargePower;
            MassBase = massBase;
            MassPower = massPower;
            Velocity = velocity;
            Deviation = Shared.Helpers.MathHelpers.DegreesToRadians(deviation);
            VelocityDirection = velocityDirection;
            Environment = environmentSetting;
            Color = color;
        }

		public InputVariant Variant { get; set; }
        public PrimaryPlaneChargePolarity PrimaryPlaneChargePolarity { get; set; }
        public float PrimaryVoltage { get; set; }
        public float PrimaryPlaneDistance { get; set; }
        public SecondaryPlaneChargePolarity SecondaryPlaneChargePolarity { get; set; }
        public float SecondaryVoltage { get; set; }
        public float SecondaryPlaneDistance { get; set; }
        public ChargePolarity ChargePolarity { get; set; }
        public float ChargeBase { get; set; }
        public float ChargePower { get; set; }
        public float MassBase { get; set; }
        public float MassPower { get; set; }
        public float Velocity { get; set; }
        public float Deviation { get; set; }
        public VelocityDirection VelocityDirection { get; set; }
        public EnvironmentSetting Environment { get; set; }
    }
}
