﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class EasyWavePhysicsService : IWavePhysicsService
	{
		private readonly WaveInfo _waveInfo = new WaveInfo()
		{
			Amplitude = 1,
			T = 1,
			WaveLength = 1//2 * (float)Math.PI
		};

		private readonly BounceType _leftEndBounce;
		private readonly float _rightEndDistance;

		public EasyWavePhysicsService(BounceType leftEndBounce, float rightEndDistance)
		{
			_leftEndBounce = leftEndBounce;
			_rightEndDistance = rightEndDistance;
		}

		public float Amplitude => _waveInfo.Amplitude;

		public float MinX => 0;

		public float MaxX => _rightEndDistance;

		public float WaveLength => _waveInfo.WaveLength;

		public bool HasWavePackage => true;

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

		public float? CalculateWavePackageY(float x, float time)
		{
			if (x < 0 || x > _rightEndDistance)
			{
				return null;
			}

			var leftEndConstant = _leftEndBounce == BounceType.Oscillating ? 0 : 0.5;
			return 2 * _waveInfo.Amplitude *
				(float)Math.Sin(leftEndConstant * Math.PI + 
					2 * Math.PI *
					(x + _waveInfo.WaveLength / 4) / _waveInfo.WaveLength);
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
			var leftEndConstant = _leftEndBounce == BounceType.Oscillating ? 0 : 0.5;

			var y =
				_waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					((time / _waveInfo.T) + ((x + leftEndConstant) / _waveInfo.WaveLength)));

			return (float)y;
		}

		public float? CalculateWavePackage(float x, float time)
		{
			if (x > _rightEndDistance || x < 0)
			{
				return null;
			}

			var leftEndConstant = _leftEndBounce == BounceType.Oscillating ? 0 : 0.5;

			var y1 =
				2 * _waveInfo.Amplitude *
				Math.Sin(
					2 * Math.PI *
					(leftEndConstant +
					x + _waveInfo.WaveLength / 4) / _waveInfo.WaveLength);

			return (float)Math.Abs(y1);
		}

		public float CalculateFirstWaveMinX(float time) => MinX;

		public float CalculateFirstWaveMaxX(float time) => MaxX;

		public float CalculateSecondWaveMinX(float time) => MinX;

		public float CalculateSecondWaveMaxX(float time) => MaxX;
	}
}
