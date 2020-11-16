using Physics.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using ExtendedNumerics;

namespace Physics.ElectricParticle.Logic
{
    public class PhysicsService : IPhysicsService
    {
        private readonly MotionSetup _setup;

        public PhysicsService(MotionSetup motionSetup)
        {
            _setup = motionSetup;
        }

        public BigDecimal MaxT => 30;

		public BigDecimal PrimaryAxisCoordinate(BigDecimal t12)
		{
			var t = 0.000000001M;
			decimal left = (decimal)_setup.Velocity * (decimal)Math.Cos(_setup.Deviation) * t;
			decimal upperFraction = (decimal)_setup.PrimaryPlaneChargePolarity * (decimal)_setup.PrimaryVoltage * (decimal)_setup.ChargePolarity * ((decimal)_setup.ChargeBase * 1) * (t * t);
			decimal lowerFraction = (2 * ((decimal)_setup.MassBase * 1 ) * (decimal)_setup.PrimaryPlaneDistance * (decimal)_setup.Environment.Value);
			BigDecimal fraction = upperFraction / lowerFraction;
			fraction *= new BigDecimal(1, (int)(_setup.ChargePower-_setup.MassPower));
			return left + fraction;
		}

		public BigDecimal SecondaryAxisCoordinate(BigDecimal t)
		{
			BigDecimal y = _setup.Velocity * (BigDecimal)Math.Sin(_setup.Deviation) * t + (((float)_setup.SecondaryPlaneChargePolarity * _setup.SecondaryVoltage * (float)_setup.ChargePolarity * (_setup.ChargeBase * Math.Pow(10, _setup.ChargePower)) * (t * t)) / (2 * (_setup.MassBase * Math.Pow(10, _setup.MassPower)) * _setup.SecondaryPlaneDistance * _setup.Environment.Value));
			return y;
		}

		public BigDecimal ComputeX(BigDecimal t)
        {
			switch(_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return PrimaryAxisCoordinate(t);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return SecondaryAxisCoordinate(t);
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisCoordinate(t);
			}
            return 0;
        }

        public BigDecimal ComputeY(BigDecimal t)
        {
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return SecondaryAxisCoordinate(t);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return PrimaryAxisCoordinate(t);
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisCoordinate(t);
			}
			return 0;
        }
    }
}
