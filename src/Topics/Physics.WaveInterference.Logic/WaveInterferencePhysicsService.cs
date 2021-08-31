using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.WaveInterference.Logic
{
	public class WaveInterferencePhysicsService : IWavePhysicsService
	{
		private WavePhysicsService[] _physicsServices;

		public WaveInterferencePhysicsService(params WavePhysicsService[] physicsServices)
		{
			_physicsServices = physicsServices;
		}

		public float? CalculateY(float x, float time)
		{
			var wave1 = _physicsServices[0].CalculateY(x, time);
			var wave2 = _physicsServices[1].CalculateY(x, time);
			if (wave1 == null || wave2 == null)
			{
				return null;
			}

			return wave1.Value + wave2.Value;
		}
	}
}
