using Physics.Shared.Mathematics;
using Xunit;

namespace Physics.ElectricParticle.Logic.Tests
{
	public class PhysicsServiceTests
	{
		[Fact]
		public void Example1_Velocity()
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
	}
}
