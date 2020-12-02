using Xunit;

namespace Physics.CompoundOscillations.Logic.Tests
{
	public class OscillationPhysicsServiceTests
    {
		[InlineData(0, 0)]
		[InlineData(0.01, 0.06279051953)]
		[InlineData(1.09, 0.535826795)]
		[Theory]
		public void SimpleSinusoid(double time, double expectedY) => VerifySingleOscillation(1, 1, 0, time, expectedY);

		[InlineData(0, -1.397077491)]
		[InlineData(0.01, -0.7843548837)]
		[InlineData(0.48, -2.547108934)]
		[Theory]
		public void SpencerA(double time, double expectedY) => VerifySingleOscillation(5, 2, 6, time, expectedY);

		[InlineData(0, 0.2822400161)]
		[InlineData(0.15, -0.2822400161)]
		[InlineData(0.53, -1.970294591)]
		[Theory]
		public void HillA(double time, double expectedY) => VerifySingleOscillation(2, 10, 3, time, expectedY);

		private void VerifySingleOscillation(double amplitude, double frequence, double phase, double time, double expectedY)
		{
			var oscillationInfo = new OscillationInfo("Test", amplitude, frequence, phase, "#000000");
			var physicsService = new OscillationPhysicsService(oscillationInfo);
			var actualY = physicsService.CalculateY(time);
			Assert.Equal(expectedY, actualY, 4);
		}
	}
}
