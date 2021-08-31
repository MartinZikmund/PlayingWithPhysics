using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.WaveInterference.Logic
{
	public class WaveInterferencePhysicsService : IWavePhysicsService
	{
		private WavePhysicsService[] _physicsServices;
		public WaveInterferencePhysicsService(WavePhysicsService[] physicsServices)
		{
			_physicsServices = physicsServices;
		}
		public double CalculateY(float x, float time)
		{
			double result = _physicsServices[0].CalculateY(x, time) + _physicsServices[1].CalculateY(x, time);
			return result;
		}
	}
}
