using System;
using Physics.Shared.Mathematics;
using Xunit;

namespace Physics.ElectricParticle.Logic.Tests
{
	public class PhysicsServiceTests
	{
		[Fact]
		public void Example1_XY2()
		{
			var motionSetup = new ElectricParticleSimulationSetup(
				InputVariant.EasyHorizontalNoGravity,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				null,
				new ParticleSetup(ParticleType.AtomNucleus, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var x = physicsService.ComputeX(new BigNumber(0, 0));
			var y = physicsService.ComputeY(new BigNumber(0, 0));
			Assert.Equal(0, x);
			Assert.Equal(0, y);
		}

		[Fact]
		public void Example1_Velocity27()
		{
			var motionSetup = new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var v = physicsService.ComputeV(new BigNumber(2.5, -8));
			Assert.True(Math.Abs((double)(21978522 - v)) < 1);
		}

		[Fact]
		public void Example1_Acceleration22()
		{
			var motionSetup = new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var a = physicsService.ComputeA(new BigNumber(2.5, -8));
			AlmostEqual(new BigNumber(8.79, 14), a);
		}

		[Fact]
		public void Example1_Ek12()
		{
			var motionSetup = new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var ek = physicsService.ComputeEk(new BigNumber(1.0, -8));
			AlmostEqual(new BigNumber(3.5, -17), ek, 1);
		}

		[Fact]
		public void Example2_Ep15()
		{
			var motionSetup = new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var ep = physicsService.ComputeEp(new BigNumber(1.3, -8));
			AlmostEqual(new BigNumber(-1.39, -16), ep, 2);
		}

		private static void AlmostEqual(BigNumber expected, BigNumber actual, int precision = 2)
		{
			var normalizedExpected = expected.Normalize();
			var normalizedActual = actual.Normalize();

			Assert.Equal(normalizedExpected.Exponent, normalizedActual.Exponent);
			Assert.Equal(normalizedExpected.Mantisa, normalizedActual.Mantisa, precision);
		}
	}
}
