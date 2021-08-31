using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.WaveInterference.Logic
{
	public class WavePhysicsService : IWavePhysicsService
	{
		public float Delta = 0.1f;
		public float X = -20f;
		public WaveInfo _wave;
		public WavePhysicsService(WaveInfo wave)
		{
			_wave = wave;
		}

		public float MaxY => Math.Abs(_wave.Amplitude);

		public float MinY => -Math.Abs(_wave.Amplitude);

		public double CalculateA(float x, float time)
		{
			double amplitude = _wave.Amplitude * Math.Sin(2 * Math.PI * ((time / _wave.Period) - (double)_wave.Direction * ((x + _wave.SourceDistance) / _wave.WaveLength)));
			return amplitude;
		}
	}
}
