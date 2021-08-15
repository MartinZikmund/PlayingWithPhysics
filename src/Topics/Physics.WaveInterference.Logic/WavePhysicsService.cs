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
		public double CalculateA(float x)
		{
			double amplitude = _wave.Amplitude * Math.Sin(2 * Math.PI * ((_wave.TimeSinceStart / _wave.Period) - (double)_wave.Direction * (x / _wave.WaveLength)));
			return amplitude;
		}
	}
}
