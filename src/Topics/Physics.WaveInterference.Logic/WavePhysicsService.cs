using System;

namespace Physics.WaveInterference.Logic
{
	public class WavePhysicsService : IWavePhysicsService
	{
		public WaveInfo _wave;
		public WavePhysicsService(WaveInfo wave)
		{
			_wave = wave;
		}

		public float StartX => _wave.Direction == WaveDirection.Right ? _wave.OriginX : float.NegativeInfinity;

		public float EndX => _wave.Direction == WaveDirection.Left ? _wave.OriginX : float.PositiveInfinity;

		public float MaxY => Math.Abs(_wave.Amplitude);

		public float MinY => -Math.Abs(_wave.Amplitude);

		public WaveInfo Wave => _wave;

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


			return (float)(_wave.Amplitude * Math.Sin(2 * Math.PI * ((time / _wave.Period) - (double)_wave.Direction * ((x + _wave.OriginX) / _wave.WaveLength))));
		}
	}
}
