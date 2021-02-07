using System;
using ExtendedNumerics;
using Physics.Shared.Logic.Constants;
using Physics.Shared.Mathematics;

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

		private readonly ElectricParticleSimulationSetup _setup;

		public PhysicsService(ElectricParticleSimulationSetup motionSetup)
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

		public BigNumber ComputeX(BigNumber time)
		{
			var goniometricAngle = Math.Cos(_setup.Particle.StartVelocityDeviation);
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.EasyHorizontalNoGravity:
					return SimpleAxisCoordinate(time, goniometricAngle);
				case InputVariant.EasyHorizontalWithGravity:
					return 0;
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
			}
			return 0;
		}

		public BigNumber ComputeY(BigNumber time)
		{
			var goniometricAngle = Math.Sin(_setup.Particle.StartVelocityDeviation);
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return SimpleAxisCoordinate(time, goniometricAngle);
				case InputVariant.EasyHorizontalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane);
				case InputVariant.EasyHorizontalWithGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane) - GravityAcceleration(time);
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return SimpleAxisCoordinate(time, goniometricAngle) - GravityAcceleration(time);
			}
			return 0;
		}

		/// <summary>
		/// Uses a complex equation to calculate axis coordinate.
		/// </summary>
		/// <param name="time">Current time.</param>
		/// <param name="goniometricAngle">Either Cos or Sin of the start velocity deviation.</param>
		/// <param name="plane">Plane we are currently working with.</param>
		/// <returns></returns>
		private BigNumber ComplexAxisCoordinate(
			BigNumber time,
			double goniometricAngle,
			PlaneSetup plane)
		{
			// Left side of addition
			var left = _setup.Particle.StartVelocity * goniometricAngle * time;

			// Right side of addition
			var time2 = time * time;
			var upperFraction = (int)plane.Polarity * plane.Voltage * (int)_setup.Particle.Polarity * GetChargeBase() * time2;
			var lowerFraction = 2 * (GetMassBase() * plane.Distance * _setup.Environment.Value);
			var fraction = upperFraction / lowerFraction;
			var right = fraction * new BigNumber(1, GetChargeExponent() - GetMassExponent());

			// Outer addition
			return left + right;
		}

		private BigNumber SimpleAxisCoordinate(
			BigNumber time,
			double goniometricAngle)
		{
			return _setup.Particle.StartVelocity * goniometricAngle * time;
		}

		private BigNumber GravityAcceleration(BigNumber time)
		{
			return 0.5f * GravityConstants.Earth * time * time;
		}

		public BigNumber SimpleCosAxisCoordinate(BigNumber time) => _setup.Particle.StartVelocity * Math.Sin(_setup.Particle.StartVelocityDeviation) * time;

		public BigNumber SimpleSinAxisCoordinate(BigNumber time) => _setup.Particle.StartVelocity * Math.Cos(_setup.Particle.StartVelocityDeviation) * time;

		#endregion

		#region V, Vx, Vy, A

		public BigNumber ComputeA(BigNumber time)
		{
			//switch (_setup.Variant)
			//{
			//	case InputVariant.EasyVerticalNoGravity:
			//		return ComplexAcceleration(time, _setup.VerticalPlane);
			//	case InputVariant.EasyHorizontalNoGravity:
			//		return ComplexAcceleration(time, _setup.HorizontalPlane);
			//	case InputVariant.EasyHorizontalWithGravity:
			//		return ComplexAcceleration(time, _setup.HorizontalPlane);
			//	case InputVariant.AdvancedVerticalWithGravity:
			//		return ComplexAxisCoordinate(time, goniometricAngle, _setup.VerticalPlane);
			//	case InputVariant.AdvancedVerticalHorizontalNoGravity:
			//		return ComplexAxisCoordinate(time, goniometricAngle, _setup.HorizontalPlane);
			//}
			return 0;
		}

		private BigNumber ComplexAcceleration(
			BigNumber time,
			PlaneSetup plane)
		{
			var upperFraction = (int)plane.Polarity * plane.Voltage * (int)_setup.Particle.Polarity * GetChargeBase();
			var lowerFraction = GetMassBase() * plane.Distance * _setup.Environment.Value;
			return upperFraction / lowerFraction;
		}

		private BigNumber ComplexAccelerationX(
			BigNumber time,
			PlaneSetup plane)
		{
			return 0;
		}

		private BigNumber ComplexAccelerationY(
			BigNumber time,
			PlaneSetup plane)
		{
			return 0;
		}
		#endregion
	}
}
