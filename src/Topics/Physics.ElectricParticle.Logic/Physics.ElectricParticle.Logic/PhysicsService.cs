﻿using System;
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

		public BigNumber ComputeVx(BigNumber time)
		{
			var goniometricAngle = Math.Cos(_setup.Particle.StartVelocityDeviation);
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return ComplexVelocityAxis(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.EasyHorizontalNoGravity:
					return SimpleVelocityAxis(goniometricAngle);
				case InputVariant.EasyHorizontalWithGravity:
					return 0;
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexVelocityAxis(time, goniometricAngle, _setup.VerticalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return ComplexVelocityAxis(time, goniometricAngle, _setup.VerticalPlane);
			}
			return 0;
		}

		public BigNumber ComputeVy(BigNumber time)
		{
			var goniometricAngle = Math.Sin(_setup.Particle.StartVelocityDeviation);
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return SimpleVelocityAxis(goniometricAngle);
				case InputVariant.EasyHorizontalNoGravity:
					return ComplexVelocityAxis(time, goniometricAngle, _setup.HorizontalPlane);
				case InputVariant.EasyHorizontalWithGravity:
					return ComplexVelocityAxis(time, goniometricAngle, _setup.HorizontalPlane) - GravityVelocity(time);
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexVelocityAxis(time, goniometricAngle, _setup.HorizontalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return SimpleVelocityAxis(goniometricAngle) - GravityVelocity(time);
			}
			return 0;
		}

		public BigNumber ComputeV(BigNumber time)
		{
			var vx = ComputeVx(time);
			var vy = ComputeVy(time);			
			if (vx.Mantisa == 0)
			{
				return vy;
			}
			else if (vy.Mantisa == 0)
			{
				return vx;
			}
			var vx2 = vx * vx;
			var vy2 = vy * vy;
			return Math.Sqrt((double)(vx2 + vy2));
		}

		private BigNumber ComplexVelocityAxis(
			BigNumber time,
			double goniometricAngle,
			PlaneSetup plane)
		{
			// Left side of addition
			var left = _setup.Particle.StartVelocity * goniometricAngle;

			// Right side of addition
			var upperFraction = (int)plane.Polarity * plane.Voltage * (int)_setup.Particle.Polarity * GetChargeBase() * time;
			var lowerFraction = GetMassBase() * plane.Distance * _setup.Environment.Value;
			var fraction = upperFraction / lowerFraction;
			var right = fraction * new BigNumber(1, GetChargeExponent() - GetMassExponent());

			// Outer addition
			return left + right;
		}

		private BigNumber SimpleVelocityAxis(double goniometricAngle)
		{
			return _setup.Particle.StartVelocity * goniometricAngle;
		}

		private BigNumber GravityVelocity(BigNumber time)
		{
			return GravityConstants.Earth * time;
		}

		public BigNumber ComputeAx(BigNumber time)
		{
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return ComplexAccelerationAxis(time, _setup.VerticalPlane);
				case InputVariant.EasyHorizontalNoGravity:
					return 0;
				case InputVariant.EasyHorizontalWithGravity:
					return 0;
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexAccelerationAxis(time, _setup.VerticalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return ComplexAccelerationAxis(time, _setup.VerticalPlane);
			}
			return 0;
		}

		public BigNumber ComputeAy(BigNumber time)
		{
			switch (_setup.Variant)
			{
				case InputVariant.EasyVerticalNoGravity:
					return 0;
				case InputVariant.EasyHorizontalNoGravity:
					return ComplexAccelerationAxis(time, _setup.HorizontalPlane);
				case InputVariant.EasyHorizontalWithGravity:
					return ComplexAccelerationAxis(time, _setup.HorizontalPlane) - GravityConstants.Earth;
				case InputVariant.AdvancedVerticalHorizontalNoGravity:
					return ComplexAccelerationAxis(time, _setup.HorizontalPlane);
				case InputVariant.AdvancedVerticalWithGravity:
					return -GravityConstants.Earth;
			}
			return 0;
		}

		public BigNumber ComputeA(BigNumber time)
		{
			var ax = ComputeAx(time);
			var ax2 = ax * ax;
			var ay = ComputeVy(time);
			var ay2 = ay * ay;
			if (ax.Mantisa == 0)
			{
				return ay;
			}
			else if (ay.Mantisa == 0)
			{
				return ax;
			}
			return Math.Sqrt((double)(ax2 + ay2));
		}

		private BigNumber ComplexAccelerationAxis(
			BigNumber time,
			PlaneSetup plane)
		{
			var upperFraction = (int)plane.Polarity * plane.Voltage * (int)_setup.Particle.Polarity * GetChargeBase();
			var lowerFraction = GetMassBase() * plane.Distance * _setup.Environment.Value;
			var fraction = upperFraction / lowerFraction;
			return fraction * new BigNumber(1, GetChargeExponent() - GetMassExponent());
		}

		public BigNumber ComputeE(BigNumber time)
		{
			return ComputeEk(time) + ComputeEp(time);
		}

		public BigNumber ComputeEk(BigNumber time)
		{
			var v = ComputeV(time);
			var v2 = v * v;
			return 0.5f * GetMassBase() * v2 * new BigNumber(1, GetMassExponent());
		}

		public BigNumber ComputeEp(BigNumber time)
		{
			BigNumber horizontalEp = 0;
			if (_setup.VerticalPlane != null)
			{
				horizontalEp = ComplexEpAxis(_setup.VerticalPlane, ComputeX(time));
			}
			BigNumber verticalEp = 0;
			if (_setup.HorizontalPlane != null)
			{
				verticalEp = ComplexEpAxis(_setup.HorizontalPlane, ComputeY(time));
			}
			return horizontalEp + verticalEp;
		}

		private BigNumber ComplexEpAxis(PlaneSetup plane, BigNumber coordinate)
		{
			BigNumber top = -plane.Voltage * GetChargeBase();
			BigNumber bottom = plane.Distance * _setup.Environment.Value;
			var fraction = top / bottom;
			fraction *= new BigNumber(1, GetChargeExponent());

			var right = plane.Distance / 2 + coordinate;

			return fraction * right;
		}
	}
}
