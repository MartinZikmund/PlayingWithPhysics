using Xunit;

namespace Physics.StationaryWaves.Logic.Tests
{
    public class AdvancedWavePhysicsServiceTests
    {
		[InlineData(0.0f, 0.22f, 0.98f)]
		[InlineData(0.3f, 0.22f, null)]
		[InlineData(0.2f, 0.8f, -0.59f)]
		[InlineData(0.72f, 0.8f, 0.48f)]
		[InlineData(0.94f, 0.8f, null)]
		[InlineData(0.0f, 1.43f, 0.43f)]
		[InlineData(0.79f, 1.43f, -0.77f)]
		[InlineData(0.94f, 1.43f, 0.06f)]
		[Theory]
        public void StaticBounceFirstWave(float x, float time, float? expected)
        {
			var service = new AdvancedWavePhysicsService(BounceType.Static);

			var actual = service.CalculateFirstWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2);
			}
        }

		[InlineData(0.0f, 0.28f, null)]
		[InlineData(0.6f, 0.28f, null)]
		[InlineData(1, 0.28f, null)]
		[InlineData(0.0f, 0.8f, null)]
		[InlineData(0.6f, 0.8f, null)]
		[InlineData(1, 0.8f, null)]
		[InlineData(0.0f, 1f, null)]
		[InlineData(0.6f, 1f, null)]
		[InlineData(1, 1f, 0f)]
		[InlineData(1, 1.52f, 0.13f)]
		[InlineData(0.74f, 1.52f, -1.00f)]
		[InlineData(0.5f, 1.52f, -0.13f)]
		[InlineData(0.2f, 1.52f, null)]
		[InlineData(0f, 2.16f, -0.84f)]
		[InlineData(0.2f, 2.16f, -0.77f)]
		[InlineData(0.74f, 2.16f, 0.59f)]
		[Theory]
		public void StaticBounceSecondWave(float x, float time, float? expected)
		{
			var service = new AdvancedWavePhysicsService(BounceType.Static);

			var actual = service.CalculateSecondWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2);
			}
		}

		[InlineData(0f, 0f, null)]
		[InlineData(0.76f, 0.76f, null)]
		[InlineData(0.76f, 1f, null)]
		[InlineData(1f, 1f, 0.0f)]
		[InlineData(1f, 1.56f, 0.0f)]
		[InlineData(0.86f, 1.56f, -1.43f)]
		[InlineData(0.23f, 1.56f, null)]
		[InlineData(0.1f, 2.16f, -0.63f)]
		[Theory]
		public void StaticBounceCompoundWave(float x, float time, float? expected)
		{
			var service = new AdvancedWavePhysicsService(BounceType.Static);

			var actual = service.CalculateCompoundY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2);
			}
		}

		[InlineData(0.0f, 0.24f, 1f)]
		[InlineData(0.3f, 0.24f, null)]
		[InlineData(0.2f, 0.72f, -0.13f)]
		[InlineData(0.72f, 0.72f, 0.0f)]
		[InlineData(0.94f, 0.72f, null)]
		[InlineData(0.0f, 1.4f, 0.59f)]
		[InlineData(0.79f, 1.4f, -0.64f)]
		[InlineData(0.94f, 1.4f, 0.25f)]
		[Theory]
		public void OscillatingBounceFirstWave(float x, float time, float? expected)
		{
			var service = new AdvancedWavePhysicsService(BounceType.Oscillating);

			var actual = service.CalculateFirstWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2);
			}
		}

		[InlineData(0.0f, 0.2f, null)]
		[InlineData(0.6f, 0.2f, null)]
		[InlineData(1, 0.2f, null)]
		[InlineData(0.0f, 0.8f, null)]
		[InlineData(0.6f, 0.8f, null)]
		[InlineData(1, 0.8f, null)]
		[InlineData(0.0f, 1f, null)]
		[InlineData(0.6f, 1f, null)]
		[InlineData(1f, 1f, 0f)]
		[InlineData(1, 1.4f, 0.59f)]
		[InlineData(0.74f, 1.4f, 0.77f)]
		[InlineData(0.5f, 1.4f, null)]
		[InlineData(0.2f, 1.4f, null)]
		[InlineData(0f, 2.4f, 0.59f)]
		[InlineData(0.2f, 2.4f, -0.59f)]
		[InlineData(0.69f, 2.4f, 0.54f)]
		[Theory]
		public void OscillatingBounceSecondWave(float x, float time, float? expected)
		{
			var service = new AdvancedWavePhysicsService(BounceType.Oscillating);

			var actual = service.CalculateSecondWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2);
			}
		}

		[InlineData(0f, 0f, null)]
		[InlineData(0.76f, 0.76f, null)]
		[InlineData(0.76f, 1f, null)]
		[InlineData(1f, 1f, 0.0f)]
		[InlineData(1f, 1.26f, 2f)]
		[InlineData(0.86f, 1.26f, 1.27f)]
		[InlineData(0.23f, 1.26f, null)]
		[InlineData(0.1f, 2.2f, 1.54f)]
		[Theory]
		public void OscillatingBounceCompoundWave(float x, float time, float? expected)
		{
			var service = new AdvancedWavePhysicsService(BounceType.Oscillating);

			var actual = service.CalculateCompoundY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2);
			}
		}
	}
}
