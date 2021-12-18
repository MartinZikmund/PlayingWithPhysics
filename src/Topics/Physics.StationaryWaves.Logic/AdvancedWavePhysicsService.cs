using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class AdvancedWavePhysicsService : IWavePhysicsService
	{
		private const float XMax = 1;

		private readonly WaveInfo _waveInfo = new WaveInfo()
		{
			Amplitude = 1,
			WaveLength = 1,
			T = 1
		};

		private readonly BounceType _bounceType;

		public AdvancedWavePhysicsService(BounceType bounceType)
		{
			_bounceType = bounceType;
		}

		public float? CalculateWaveFrontX(float time) =>
			_waveInfo.WaveLength * time / _waveInfo.T;

		public float? CalculateCompoundY(float x, float time)
		{
			var firstWaveY = CalculateFirstWaveY(x, time);
			var secondWaveY = CalculateSecondWaveY(x, time);
			if (firstWaveY == null || secondWaveY == null)
			{
				return null;
			}

			return firstWaveY.Value + secondWaveY.Value;
		}

		public float? CalculateFirstWaveY(float x, float time)
		{
			if (x > CalculateWaveFrontX(time))
			{
				return null;
			}

			var y =
				_waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					((time / _waveInfo.T) - (x / _waveInfo.WaveLength)));

			return (float)y;
		}

		public float? CalculateSecondWaveY(float x, float time)
		{
			if (2 * XMax - x > CalculateWaveFrontX(time))
			{
				return null;
			}

			var bounceConstant = _bounceType == BounceType.Static ? 0.5f : 0f;

			var y =
				_waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					((time / _waveInfo.T) - (2 * XMax - x + bounceConstant) / _waveInfo.WaveLength));

			return (float)y;
		}
	}
}
