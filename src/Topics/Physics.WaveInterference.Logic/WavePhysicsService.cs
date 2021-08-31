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

		public double CalculateY(float distanceFromOrigin, float time) =>
			_wave.Amplitude * Math.Sin(2 * Math.PI * ((time / _wave.Period) + (double)_wave.Direction * ((distanceFromOrigin + _wave.OriginX) / _wave.WaveLength)));
	}
}
