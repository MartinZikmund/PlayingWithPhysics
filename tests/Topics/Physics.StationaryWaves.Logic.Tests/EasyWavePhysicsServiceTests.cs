using System;
using Xunit;

namespace Physics.StationaryWaves.Logic.Tests
{
	public class EasyWavePhysicsServiceTests
    {
		[InlineData(0.0f, 0.24f, (float)Math.PI, 1f)]
		[InlineData(0.3f, 0.24f, (float)Math.PI, 0.93f)]
		[InlineData(2.9f, 0.24f, (float)Math.PI, -0.98f)]
		[InlineData(0.2f, 0.64f, (float)Math.PI, -0.63f)]
		[InlineData(2.5f, 0.64f, (float)Math.PI, 1f)]
		[InlineData(3.6f, 0.64f, (float)Math.PI, 0.41f)]
		[InlineData(5.5f, 2.04f, (float)Math.PI, 0.86f)]
		[InlineData(6.3f, 2.04f, (float)Math.PI, 0.23f)]
		[InlineData(1.5f, 2.04f, (float)(3 * Math.PI / 2), -0.95f)]
		[InlineData(1.7f, 0.91f, (float)(3 * Math.PI / 2), -0.77f)]
		[Theory]
		public void StaticBounceFirstWave(float x, float time, float rightEndDistance, float? expected)
		{
			var service = new EasyWavePhysicsService(BounceType.Static, rightEndDistance);

			var actual = service.CalculateFirstWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2f);
			}
		}

		[InlineData(0.0f, 0.24f, (float)Math.PI, -1f)]
		[InlineData(0.3f, 0.24f, (float)Math.PI, -0.97f)]
		[InlineData(2.9f, 0.24f, (float)Math.PI, 0.95f)]
		[InlineData(0.2f, 0.64f, (float)Math.PI, 0.88f)]
		[InlineData(2.5f, 0.64f, (float)Math.PI, -0.24f)]
		[InlineData(3.6f, 0.64f, (float)Math.PI, -0.97f)]
		[InlineData(5.5f, 2.04f, (float)Math.PI, 0.51f)]
		[InlineData(6.3f, 2.04f, (float)Math.PI, -0.26f)]
		[InlineData(1.5f, 2.04f, (float)(3 * Math.PI / 2), -0.98f)]
		[InlineData(1.7f, 0.91f, (float)(3 * Math.PI / 2), -0.91f)]
		[Theory]
		public void StaticBounceSecondWave(float x, float time, float rightEndDistance, float? expected)
		{
			var service = new EasyWavePhysicsService(BounceType.Static, rightEndDistance);

			var actual = service.CalculateSecondWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2f);
			}
		}

		[InlineData(0.0f, 0.24f, (float)Math.PI, 1f)]
		[InlineData(0.3f, 0.24f, (float)Math.PI, 0.93f)]
		[InlineData(2.9f, 0.24f, (float)Math.PI, -0.98f)]
		[InlineData(0.2f, 0.64f, (float)Math.PI, -0.63f)]
		[InlineData(2.5f, 0.64f, (float)Math.PI, 1f)]
		[InlineData(3.6f, 0.64f, (float)Math.PI, 0.41f)]
		[InlineData(5.5f, 2.04f, (float)Math.PI, 0.86f)]
		[InlineData(6.3f, 2.04f, (float)Math.PI, 0.23f)]
		[InlineData(1.5f, 2.04f, (float)(3 * Math.PI / 2), -0.95f)]
		[InlineData(1.7f, 0.91f, (float)(3 * Math.PI / 2), -0.77f)]
		[Theory]
		public void OscillatingBounceFirstWave(float x, float time, float rightEndDistance, float? expected)
		{
			var service = new EasyWavePhysicsService(BounceType.Oscillating, rightEndDistance);

			var actual = service.CalculateFirstWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2f);
			}
		}

		[InlineData(0.0f, 0.24f, (float)Math.PI, -1f)]
		[InlineData(0.3f, 0.24f, (float)Math.PI, -0.97f)]
		[InlineData(2.9f, 0.24f, (float)Math.PI, 0.95f)]
		[InlineData(0.2f, 0.64f, (float)Math.PI, 0.88f)]
		[InlineData(2.5f, 0.64f, (float)Math.PI, -0.24f)]
		[InlineData(3.6f, 0.64f, (float)Math.PI, -0.97f)]
		[InlineData(5.5f, 2.04f, (float)Math.PI, 0.51f)]
		[InlineData(6.3f, 2.04f, (float)Math.PI, -0.26f)]
		[InlineData(1.5f, 2.04f, (float)(3 * Math.PI / 2), -0.98f)]
		[InlineData(1.7f, 0.91f, (float)(3 * Math.PI / 2), -0.91f)]
		[Theory]
		public void OscillatingBounceSecondWave(float x, float time, float rightEndDistance, float? expected)
		{
			var service = new EasyWavePhysicsService(BounceType.Oscillating, rightEndDistance);

			var actual = service.CalculateSecondWaveY(x, time);
			if (expected is null || actual is null)
			{
				Assert.Equal(expected, actual);
			}
			else
			{
				Assert.Equal(expected.Value, actual.Value, 2f);
			}
		}
	}
}
