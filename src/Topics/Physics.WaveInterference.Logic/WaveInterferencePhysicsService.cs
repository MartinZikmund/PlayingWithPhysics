using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.WaveInterference.Logic
{
	public class WaveInterferencePhysicsService : IWavePhysicsService
	{
		public float Delta = 0.1f;
		private WavePhysicsService[] _physicsServices;
		public WaveInterferencePhysicsService(WavePhysicsService[] physicsServices)
		{
			_physicsServices = physicsServices;
		}
		public double CalculateA(float x, float time)
		{
			double result = _physicsServices[0].CalculateA(x, time) + _physicsServices[1].CalculateA(x, time);
			return result;
		}
	}
}
