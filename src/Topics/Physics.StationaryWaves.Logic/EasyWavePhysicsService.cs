using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class EasyWavePhysicsService : IWavePhysicsService
	{
		private const float XMax = 2 * (float)Math.PI * 3 / 2;

		private readonly WaveInfo _waveInfo = new WaveInfo()
		{
			Amplitude = 1,
			T = 1,
			WaveLength = 2 * (float)Math.PI
		};

		private readonly BounceType _leftEndBounce;
		private readonly float _rightEndDistance;

		public EasyWavePhysicsService(BounceType leftEndBounce, float rightEndDistance)
		{
			_leftEndBounce = leftEndBounce;
			_rightEndDistance = rightEndDistance;
		}

		public float? CalculateCompoundY(float x, float time)
		{
			if (x < 0 || x > _rightEndDistance)
			{
				return null;
			}

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
			var y =
				_waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					((time / _waveInfo.T) - (x / _waveInfo.WaveLength)));

			return (float)y;
		}

		public float? CalculateSecondWaveY(float x, float time)
		{
			var leftEndConstant = _leftEndBounce == BounceType.Oscillating ? 0 : 1;

			var y =
				_waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					((time / _waveInfo.T) + ((x + leftEndConstant * Math.PI) / _waveInfo.WaveLength)));

			return (float)y;
		}

		public float? CalculateWavePackage(float x, float time)
		{
			if (x > _rightEndDistance || x < 0)
			{
				return null;
			}

			var leftEndConstant = _leftEndBounce == BounceType.Oscillating ? 0 : 1;

			var y1 =
				2 * _waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					(leftEndConstant * Math.PI / 2 +
					x + _waveInfo.WaveLength / 4) / _waveInfo.WaveLength);

			return (float)Math.Abs(y1);
		}
	}
}
