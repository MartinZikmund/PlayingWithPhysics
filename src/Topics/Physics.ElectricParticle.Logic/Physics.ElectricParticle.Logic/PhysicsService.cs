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
		#region X, Y
		public BigDecimal PrimaryAxisCoordinate(decimal time)
		{
			decimal left = (decimal)_setup.Velocity * (decimal)Math.Cos(_setup.Deviation) * time;
			decimal upperFraction = (decimal)_setup.PrimaryPlaneChargePolarity * (decimal)_setup.PrimaryVoltage * (decimal)_setup.ChargePolarity * ((decimal)_setup.ChargeBase * 1) * (time * time);
			decimal lowerFraction = (2 * ((decimal)_setup.MassBase * 1) * (decimal)_setup.PrimaryPlaneDistance * (decimal)_setup.Environment.Value);
			BigDecimal fraction = upperFraction / lowerFraction;
			fraction *= new BigDecimal(1, (int)(_setup.ChargePower - _setup.MassPower));
			return left + fraction;
		}

		public BigDecimal SecondaryAxisCoordinate(decimal time)
		{
			decimal left = (decimal)_setup.Velocity * (decimal)Math.Cos(_setup.Deviation) * time;
			decimal upperFraction = (decimal)_setup.SecondaryPlaneChargePolarity * (decimal)_setup.SecondaryVoltage * (decimal)_setup.ChargePolarity * ((decimal)_setup.ChargeBase * 1) * (time * time);
			decimal lowerFraction = (2 * ((decimal)_setup.MassBase * 1) * (decimal)_setup.SecondaryPlaneDistance * (decimal)_setup.Environment.Value);
			BigDecimal fraction = upperFraction / lowerFraction;
			fraction *= new BigDecimal(1, (int)(_setup.ChargePower - _setup.MassPower));
			return left + fraction;
		}

		public BigDecimal ComputeX(decimal time)
		{
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return PrimaryAxisCoordinate(time);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return SecondaryAxisCoordinate(time);
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisCoordinate(time);
			}
			return 0;
		}

		public BigDecimal ComputeY(decimal time)
		{
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return SecondaryAxisCoordinate(time);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return PrimaryAxisCoordinate(time);
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisCoordinate(time);
			}
			return 0;
		}
		#endregion
		#region V, Vx, Vy

		public BigDecimal PrimaryAxisVelocity(decimal time)
		{
			decimal left = (decimal)_setup.Velocity * (decimal)Math.Cos(_setup.Deviation);
			decimal upperFraction = (decimal)_setup.PrimaryPlaneChargePolarity * (decimal)_setup.PrimaryVoltage * (decimal)_setup.ChargePolarity * ((decimal)_setup.ChargeBase * 1) * time;
			decimal lowerFraction = (decimal)_setup.MassBase * (decimal)_setup.PrimaryPlaneDistance * (decimal)_setup.Environment.Value;
			BigDecimal fraction = upperFraction / lowerFraction;
			fraction *= new BigDecimal(1, (int)(_setup.ChargePower - _setup.MassPower));
			return left + fraction;
		}

		public BigDecimal SecondaryAxisVelocity(decimal time)
		{
			decimal left = (decimal)_setup.Velocity * (decimal)Math.Sin(_setup.Deviation);
			decimal upperFraction = (decimal)_setup.SecondaryPlaneChargePolarity * (decimal)_setup.SecondaryVoltage * (decimal)_setup.ChargePolarity * ((decimal)_setup.ChargeBase * 1) * time;
			decimal lowerFraction = (decimal)_setup.MassBase * (decimal)_setup.SecondaryPlaneDistance * (decimal)_setup.Environment.Value;
			BigDecimal fraction = upperFraction / lowerFraction;
			fraction *= new BigDecimal(1, (int)(_setup.ChargePower - _setup.MassPower));
			return left + fraction; ;
		}

		public BigDecimal ComputeVX(decimal time)
		{
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return PrimaryAxisVelocity(time);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return SecondaryAxisVelocity(time);
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisVelocity(time);
			}
			return 0;
		}

		public BigDecimal ComputeVY(decimal time)
		{
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return SecondaryAxisVelocity(time);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return PrimaryAxisVelocity(time);
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisVelocity(time);
			}
			return 0;
		}
		public BigDecimal ComputeV(decimal time)
		{
			BigDecimal left = ComputeVX(time);
			BigDecimal right = ComputeVY(time);
			return Math.Sqrt((double)(left * left) + (double)(right * right));
		}
		#endregion
		#region Acceleration
		//Differs with gravity, otherwise the same
		public BigDecimal PrimaryAxisAccelerationX(decimal time)
		{
			decimal upperFraction = (decimal)_setup.PrimaryPlaneChargePolarity * (decimal)_setup.PrimaryVoltage * (decimal)_setup.ChargePolarity * (decimal)_setup.ChargeBase;
			decimal lowerFraction = (decimal)_setup.MassBase * (decimal)_setup.PrimaryPlaneDistance * (decimal)_setup.Environment.Value;
			BigDecimal result = (upperFraction / lowerFraction);
			return result;
		}

		public BigDecimal PrimaryAxisAccelerationY(decimal time)
		{
			decimal upperFraction = (decimal)_setup.PrimaryPlaneChargePolarity * (decimal)_setup.PrimaryVoltage * (decimal)_setup.ChargePolarity * (decimal)_setup.ChargeBase;
			decimal lowerFraction = (decimal)_setup.MassBase * (decimal)_setup.PrimaryPlaneDistance * (decimal)_setup.Environment.Value;
			decimal right = -9.81M;
			BigDecimal result = (upperFraction / lowerFraction) + right;
			return result;
		}

		public BigDecimal ComputeAx(decimal time)
		{
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return PrimaryAxisAccelerationX(time);
				case PlaneOrientation.EasyHorizontal:
				case PlaneOrientation.EasyHorizontalWithGravity:
					return 0.0; //change to method with g=0 return
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisAccelerationX(time);
			}
			return 0;
		}

		public BigDecimal ComputeAy(decimal time)
		{
			switch (_setup.Variant)
			{
				case PlaneOrientation.EasyVertical:
					return 0.0; //change to method with g=0 return
				case PlaneOrientation.AdvancedVerticalWithGravity:
					return -9.81; //do constant
				case PlaneOrientation.EasyHorizontal:
					return PrimaryAxisAccelerationY(time);
				case PlaneOrientation.EasyHorizontalWithGravity:
					return PrimaryAxisAccelerationY(time) - 9.81;
				case PlaneOrientation.AdvancedVerticalHorizontal:
					return PrimaryAxisAccelerationY(time);
			}
			return 0;
		}

		public BigDecimal ComputeA(decimal time)
		{
			return PythagorianCalculation(ComputeAx(time), ComputeAy(time));
		}

		private BigDecimal PythagorianCalculation(BigDecimal left, BigDecimal right)
		{
			return Math.Sqrt((double)(left * left) + (double)(right * right));
		}
		#endregion
	}
}
