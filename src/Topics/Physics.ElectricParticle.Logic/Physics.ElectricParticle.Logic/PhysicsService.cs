using System;
using ExtendedNumerics;
using Physics.Shared.Math;

namespace Physics.ElectricParticle.Logic
{
	public class PhysicsService : IPhysicsService
	{
		private const float ElectronMassBase = 9.1f;
		private const int ElectronMasssExponent = -31;
		private const float ElectronChargeBase = 1.6f;
		private const int ElectronChargeExponent = -19;
		private const float NucleonMassBase = 1.67f;
		private const int NucleonMassExponent = -27;
		private const float ElementaryChargeBase = 1.6f;
		private const int ElementaryChargeExponent = -19;
		private const float ChargedBodyMassBase = 1;
		private const int ChargedBodyMassExponent = -17;		

		private readonly MotionSetup _setup;

		public PhysicsService(MotionSetup motionSetup)
		{
			_setup = motionSetup;
		}

		public BigDecimal MaxT => 30;

		public float GetMassBase()
		{
			switch (_setup.Particle.Type)
			{
				case ParticleType.ChargedBody:
					return _setup.Particle.MassMultiplier * ChargedBodyMassBase;
				case ParticleType.AtomNucleus:
					return _setup.Particle.MassMultiplier * NucleonMassBase;
				case ParticleType.Electron:
					// Electron weight is always the same
					return ElectronMassBase;
				default:
					throw new NotSupportedException("Not supported particle type");
			}
		}

		public float GetChargeBase()
		{
			switch (_setup.Particle.Type)
			{
				case ParticleType.ChargedBody:
				case ParticleType.AtomNucleus:
					return _setup.Particle.ChargeMultiplier * ElementaryChargeBase;
				case ParticleType.Electron:
					return ElectronChargeBase;
				default:
					throw new NotSupportedException("Not supported particle type");
			}
		}

		public int GetMassExponent()
		{
			switch (_setup.Particle.Type)
			{
				case ParticleType.ChargedBody:
					return ChargedBodyMassExponent;
				case ParticleType.AtomNucleus:
					return NucleonMassExponent;
				case ParticleType.Electron:
					return ElectronMasssExponent;
				default:
					throw new NotSupportedException("Not supported particle type");
			}
		}

		public int GetChargeExponent()
		{
			switch (_setup.Particle.Type)
			{
				case ParticleType.ChargedBody:
				case ParticleType.AtomNucleus:
					return ElementaryChargeExponent;
				case ParticleType.Electron:
					return ElectronChargeExponent;
				default:
					throw new NotSupportedException("Not supported particle type");
			}
		}

		#region X, Y

		//public BigNumber ComplexCosAxisCoordinate(BigNumber time)
		//{
		//	var left = (decimal)_setup.Particle.StartVelocity * (decimal)Math.Cos(_setup.Deviation) * time;
		//	var upperFraction = (decimal)_setup.HorizontalPlaneChargePolarity * (decimal)_setup.HorizontalPlaneVoltag * (decimal)_setup.ChargePolarity * ((decimal)GetChargeBase() * 1) * (time * time);
		//	var lowerFraction = (2 * ((decimal)GetMassBase() * 1) * (decimal)_setup.HorizontalPlaneDistance * (decimal)_setup.Environment.Value);
		//	BigDecimal fraction = upperFraction / lowerFraction;
		//	fraction *= new BigDecimal(1, GetChargeExponent() - GetMassExponent());
		//	return left + fraction;
		//}

		//public BigNumber ComplexSinAxisCoordinate(BigNumber time)
		//{
		//	var left = (decimal)_setup.Particle.StartVelocity * (decimal)Math.Sin(_setup.Deviation) * time;
		//	var upperFraction = (decimal)_setup.VerticalPlaneChargePolarity * (decimal)_setup.VerticalPlaneVoltage * (decimal)_setup.ChargePolarity * ((decimal)GetChargeBase() * 1) * (time * time);
		//	var lowerFraction = (2 * ((decimal)GetMassBase() * 1) * (decimal)_setup.VerticalPlaneDistance * (decimal)_setup.Environment.Value);
		//	BigDecimal fraction = upperFraction / lowerFraction;
		//	fraction *= new BigDecimal(1, GetChargeExponent() - GetMassExponent());
		//	return left + fraction;
		//}

		private BigNumber ComplexAxisCoordinate(
			BigNumber time,
			double goniometricAngle,
			PlaneSetup plane)
		{
			var time2 = time * time;
			var left = _setup.Particle.StartVelocity * goniometricAngle * time;
			var upperFraction = (int)plane.Polarity * plane.Voltage * (int)_setup.Particle.Polarity * GetChargeBase() * time2;
			var lowerFraction = 2 * (GetMassBase() * plane.Distance * _setup.Environment.Value);
			BigNumber fraction = upperFraction / lowerFraction;
			fraction *= new BigNumber(1, GetChargeExponent() - GetMassExponent());
			return left + fraction;
		}

		public BigNumber SimpleCosAxisCoordinate(BigNumber time) => _setup.Particle.StartVelocity * Math.Sin(_setup.Particle.StartVelocityDeviation) * time;

		public BigNumber SimpleSinAxisCoordinate(BigNumber time) => _setup.Particle.StartVelocity * Math.Cos(_setup.Particle.StartVelocityDeviation) * time;

		public BigNumber ComputeX(BigNumber time)
		{
			var goniometricAngle = Math.Cos(_setup.Particle.StartVelocityDeviation);
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.EasyHorizontalNoGravity:
					return SimpleCosAxisCoordinate(time);
				case InputVariant.EasyHorizontalWithGravity:
					return 0;
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane);
			}
			return 0;
		}

		public BigNumber ComputeY(BigNumber time)
		{
			var goniometricAngle = Math.Sin(_setup.Particle.StartVelocityDeviation);
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return SimpleSinAxisCoordinate(time);
				case InputVariant.AdvancedVerticalWithGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.EasyHorizontalNoGravity:
				case InputVariant.EasyHorizontalWithGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane);
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane);
			}
			return 0;
		}

		#endregion

		#region V, Vx, Vy

		//public BigDecimal PrimaryAxisVelocity(decimal time)
		//{
		//	decimal left = (decimal)_setup.Particle.StartVelocity * (decimal)Math.Cos(_setup.Deviation);
		//	decimal upperFraction = (decimal)_setup.HorizontalPlaneChargePolarity * (decimal)_setup.HorizontalPlaneVoltag * (decimal)_setup.ChargePolarity * ((decimal)GetChargeBase() * 1) * time;
		//	decimal lowerFraction = (decimal)GetMassBase() * (decimal)_setup.HorizontalPlaneDistance * (decimal)_setup.Environment.Value;
		//	BigDecimal fraction = upperFraction / lowerFraction;
		//	fraction *= new BigDecimal(1, GetChargeExponent() - GetMassExponent());
		//	return left + fraction;
		//}

		//public BigDecimal SecondaryAxisVelocity(decimal time)
		//{
		//	decimal left = (decimal)_setup.Particle.StartVelocity * (decimal)Math.Sin(_setup.Deviation);
		//	decimal upperFraction = (decimal)_setup.VerticalPlaneChargePolarity * (decimal)_setup.VerticalPlaneVoltage * (decimal)_setup.ChargePolarity * ((decimal)GetChargeBase() * 1) * time;
		//	decimal lowerFraction = (decimal)GetMassBase() * (decimal)_setup.VerticalPlaneDistance * (decimal)_setup.Environment.Value;
		//	BigDecimal fraction = upperFraction / lowerFraction;
		//	fraction *= new BigDecimal(1, GetChargeExponent() - GetMassExponent());
		//	return left + fraction;
		//}

		//public BigDecimal ComputeVX(decimal time)
		//{
		//	switch (_setup.Variant)
		//	{
		//		case InputVariant.EasyVerticalNoGravity:
		//		case InputVariant.AdvancedVerticalWithGravity:
		//			return PrimaryAxisVelocity(time);
		//		case InputVariant.EasyHorizontalNoGravity:
		//		case InputVariant.EasyHorizontalWithGravity:
		//			return SecondaryAxisVelocity(time);
		//		case InputVariant.AdvancedVerticalHorizontalNoGravity:
		//			return PrimaryAxisVelocity(time);
		//	}
		//	return 0;
		//}

		//public BigDecimal ComputeVY(decimal time)
		//{
		//	switch (_setup.Variant)
		//	{
		//		case InputVariant.EasyVerticalNoGravity:
		//		case InputVariant.AdvancedVerticalWithGravity:
		//			return SecondaryAxisVelocity(time);
		//		case InputVariant.EasyHorizontalNoGravity:
		//		case InputVariant.EasyHorizontalWithGravity:
		//			return PrimaryAxisVelocity(time);
		//		case InputVariant.AdvancedVerticalHorizontalNoGravity:
		//			return PrimaryAxisVelocity(time);
		//	}
		//	return 0;
		//}
		//public BigDecimal ComputeV(decimal time)
		//{
		//	BigDecimal left = ComputeVX(time);
		//	BigDecimal right = ComputeVY(time);
		//	return Math.Sqrt((double)(left * left) + (double)(right * right));
		//}

		//#endregion

		//#region Acceleration
		////Differs with gravity, otherwise the same
		//public BigDecimal PrimaryAxisAccelerationX(decimal time)
		//{
		//	decimal upperFraction = (decimal)_setup.HorizontalPlaneChargePolarity * (decimal)_setup.HorizontalPlaneVoltag * (decimal)_setup.ChargePolarity * (decimal)GetChargeBase();
		//	decimal lowerFraction = (decimal)GetMassBase() * (decimal)_setup.HorizontalPlaneDistance * (decimal)_setup.Environment.Value;
		//	BigDecimal result = (upperFraction / lowerFraction);
		//	return result;
		//}

		//public BigDecimal PrimaryAxisAccelerationY(decimal time)
		//{
		//	decimal upperFraction = (decimal)_setup.HorizontalPlaneChargePolarity * (decimal)_setup.HorizontalPlaneVoltag * (decimal)_setup.ChargePolarity * (decimal)GetChargeBase();
		//	decimal lowerFraction = (decimal)GetMassBase() * (decimal)_setup.HorizontalPlaneDistance * (decimal)_setup.Environment.Value;
		//	decimal right = -9.81M;
		//	BigDecimal result = (upperFraction / lowerFraction) + right;
		//	return result;
		//}

		//public BigDecimal ComputeAx(decimal time)
		//{
		//	switch (_setup.Variant)
		//	{
		//		case InputVariant.EasyVerticalNoGravity:
		//		case InputVariant.AdvancedVerticalWithGravity:
		//			return PrimaryAxisAccelerationX(time);
		//		case InputVariant.EasyHorizontalNoGravity:
		//		case InputVariant.EasyHorizontalWithGravity:
		//			return 0.0; //change to method with g=0 return
		//		case InputVariant.AdvancedVerticalHorizontalNoGravity:
		//			return PrimaryAxisAccelerationX(time);
		//	}
		//	return 0;
		//}

		//public BigDecimal ComputeAy(decimal time)
		//{
		//	switch (_setup.Variant)
		//	{
		//		case InputVariant.EasyVerticalNoGravity:
		//			return 0.0; //change to method with g=0 return
		//		case InputVariant.AdvancedVerticalWithGravity:
		//			return -9.81; //do constant
		//		case InputVariant.EasyHorizontalNoGravity:
		//			return PrimaryAxisAccelerationY(time);
		//		case InputVariant.EasyHorizontalWithGravity:
		//			return PrimaryAxisAccelerationY(time) - 9.81;
		//		case InputVariant.AdvancedVerticalHorizontalNoGravity:
		//			return PrimaryAxisAccelerationY(time);
		//	}
		//	return 0;
		//}

		//public BigDecimal ComputeA(decimal time)
		//{
		//	return PythagorianCalculation(ComputeAx(time), ComputeAy(time));
		//}

		//private BigDecimal PythagorianCalculation(BigDecimal left, BigDecimal right)
		//{
		//	return Math.Sqrt((double)(left * left) + (double)(right * right));
		//}
		#endregion
	}
}
