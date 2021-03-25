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
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var x = physicsService.ComputeX(new BigNumber(0, 0));
			var y = physicsService.ComputeY(new BigNumber(0, 0));
			Assert.Equal(0, x);
			Assert.Equal(0, y);
		}

		[Fact]
		public void Example1_XY29()
		{
			var motionSetup = new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
				new EnvironmentSetting("test", 1),
				"#000000");
			var physicsService = new PhysicsService(motionSetup);
			var x = physicsService.ComputeX(new BigNumber(2.7, -8));
			var y = physicsService.ComputeY(new BigNumber(2.7, -8));
			Assert.Equal(0.32, (double)x, 2);
			Assert.Equal(0, (double)y, 2);
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
			var a = physicsService.ComputeA();
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
		public void Example1_Ep15()
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
		
		[Fact]
		public void Example1a_Row19_X()
		{
			var setup = GetExample1a();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(1.6, -8));
			AlmostEqual(new BigNumber(-6.93f, -6), x, 2);
		}

		[Fact]
		public void Example1a_Row28_Y()
		{
			var setup = GetExample1a();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(2.5, -8));
			AlmostEqual(new BigNumber(6.25f, -6), y, 2);
		}

		[Fact]
		public void Example1b_Row12_X()
		{
			var setup = GetExample1b();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(9, -9));
			AlmostEqual(new BigNumber(3.7f, -6), x, 2);
		}

		[Fact]
		public void Example1b_Row16_Y()
		{
			var setup = GetExample1b();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(1.3, -8));
			AlmostEqual(new BigNumber(3.25f, -6), y, 2);
		}

		[Fact]
		public void Example1c_Row15_X()
		{
			var setup = GetExample1c();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(1.2, -8));
			AlmostEqual(new BigNumber(6.33f, -2), x, 2);
		}

		[Fact]
		public void Example1c_Row20_Y()
		{
			var setup = GetExample1c();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(1.7, -8));
			AlmostEqual(new BigNumber(7.36f, -6), y, 2);
		}

		[Fact]
		public void Example2a_Row19_X()
		{
			var setup = GetExample2a();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(6.4, -1));
			AlmostEqual(new BigNumber(-2.77f, 2), x, 2);
		}

		[Fact]
		public void Example2a_Row28_Y()
		{
			var setup = GetExample2a();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(1, 0));
			AlmostEqual(new BigNumber(2.5f, 2), y, 2);
		}

		[Fact]
		public void Example2b_Row12_X()
		{
			var setup = GetExample2b();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(9, -9));
			AlmostEqual(new BigNumber(3.9f, -6), x, 2);
		}

		[Fact]
		public void Example2b_Row16_Y()
		{
			var setup = GetExample2b();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(3, -8));
			AlmostEqual(new BigNumber(5.34, -6), y, 2);
		}

		[Fact]
		public void Example2c_Row15_X()
		{
			var setup = GetExample2c();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(1.1, -8));
			AlmostEqual(new BigNumber(2.75, -6), x, 2);
		}

		[Fact]
		public void Example2c_Row20_Y()
		{
			var setup = GetExample2c();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(1.4, -8));
			AlmostEqual(new BigNumber(8.62f, -2), y, 2);
		}

		[Fact]
		public void Example3a_Row19_X()
		{
			var setup = GetExample3a();
			var physicsService = new PhysicsService(setup);
			var x = physicsService.ComputeX(new BigNumber(1.9, -4));
			AlmostEqual(new BigNumber(0, 0), x, 2);
		}

		[Fact]
		public void Example3a_Row28_Y()
		{
			var setup = GetExample3a();
			var physicsService = new PhysicsService(setup);
			var y = physicsService.ComputeY(new BigNumber(2.8, -4));
			AlmostEqual(new BigNumber(1.4f, -1), y, 2);
		}

		private ElectricParticleSimulationSetup GetExample1a()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.ChargedBody, Polarity.Negative, 1, 100, 500, 150),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		private ElectricParticleSimulationSetup GetExample1b()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.AtomNucleus, Polarity.Positive, 1, 100, 500, 30),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		private ElectricParticleSimulationSetup GetExample1c()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyVerticalNoGravity,
				null,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1, 1, 500, 60),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		private ElectricParticleSimulationSetup GetExample2a()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyHorizontalNoGravity,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				null,
				new ParticleSetup(ParticleType.ChargedBody, Polarity.Negative, 1, 100, 500, 150),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		private ElectricParticleSimulationSetup GetExample2b()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyHorizontalNoGravity,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				null,
				new ParticleSetup(ParticleType.AtomNucleus, Polarity.Positive, 1, 100, 500, 30),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		private ElectricParticleSimulationSetup GetExample2c()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyHorizontalNoGravity,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				null,
				new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1, 1, 500, 60),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		private ElectricParticleSimulationSetup GetExample3a()
		{
			return new ElectricParticleSimulationSetup(
				InputVariant.EasyHorizontalWithGravity,
				new PlaneSetup(Polarity.Negative, 1000, 0.2f),
				null,
				new ParticleSetup(ParticleType.ChargedBody, Polarity.Negative, 1, 100, 500, 90),
				new EnvironmentSetting("", 1.0f),
				"#000000");
		}

		//[Fact]
		//public void Example1_DeltaT()
		//{
		//	var motionSetup = new ElectricParticleSimulationSetup(
		//		InputVariant.EasyVerticalNoGravity,
		//		null,
		//		new PlaneSetup(Polarity.Negative, 1000, 0.2f),
		//		new ParticleSetup(ParticleType.Electron, Polarity.Negative, 1.6f, 9.1f, 500, 0),
		//		new EnvironmentSetting("test", 1),
		//		"#000000");
		//	var physicsService = new PhysicsService(motionSetup);
		//	var ep = physicsService.ComputeDeltaT(500);
		//	AlmostEqual(new BigNumber(-1.39, -16), ep, 2);
		//}

		private static void AlmostEqual(BigNumber expected, BigNumber actual, int precision = 2)
		{
			var normalizedExpected = expected.Normalize();
			var normalizedActual = actual.Normalize();

			Assert.Equal(normalizedExpected.Exponent, normalizedActual.Exponent);
			Assert.Equal(normalizedExpected.Mantisa, normalizedActual.Mantisa, precision);
		}
	}
}
