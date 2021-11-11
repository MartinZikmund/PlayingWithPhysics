using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class EasyWavePhysicsService : IWavePhysicsService
	{
		public const double MaxTime = 6 * Math.PI;
		public List<(float Time, float Y)> Values = new List<(float, float)>();

		public EasyWavePhysicsService(WaveInfo wave)
		{
			Wave = wave;
		}

		public float CalculateFirstWaveY(float x, float time)
		{
			//y = 3.sin⁡(t - x)
			return 3.0f * (float)Math.Sin(time - x);
		}


		public float CalculateSecondWaveY(float x, float time)
		{
			//y = k.3.sin⁡(t + x)
			return (float)Wave.SelectedBouncingPoint * 3.0f * (float)Math.Sin(time + x);
		}

		public WaveInfo Wave { get; set; }
	}
}
