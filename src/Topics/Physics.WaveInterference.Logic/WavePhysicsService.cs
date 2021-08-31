using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.WaveInterference.Logic
{
	public class WavePhysicsService : IWavePhysicsService
	{
		public WaveInfo _wave;
		public WavePhysicsService(WaveInfo wave)
		{
			_wave = wave;
		}

		public float MaxY => Math.Abs(_wave.Amplitude);

		public float MinY => -Math.Abs(_wave.Amplitude);

		public float? CalculateY(float x, float time)
		{
			// Can this wave have a value in this location?
			if (_wave.Direction == WaveDirection.Left)
			{
				if (x > _wave.OriginX)
				{
					return null;
				}
			}

			if (_wave.Direction == WaveDirection.Right)
			{
				if (x < _wave.OriginX)
				{
					return null;
				}
			}


			return (float)(_wave.Amplitude * Math.Sin(2 * Math.PI * ((time / _wave.Period) + (double)_wave.Direction * ((x + _wave.OriginX) / _wave.WaveLength))));
		}
	}
}
