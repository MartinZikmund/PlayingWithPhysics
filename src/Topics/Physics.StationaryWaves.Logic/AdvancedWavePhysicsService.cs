using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class AdvancedWavePhysicsService : IWavePhysicsService
	{
		public WaveInfo Wave { get; set; }
		private const float B = 1;
		public float MaxY => Math.Abs(Wave.Amplitude);

		public float MinY => -Math.Abs(Wave.Amplitude);

		public AdvancedWavePhysicsService(WaveInfo wave)
		{
			Wave = wave;
		}

		public float CalculateCompoundY(float x, float time)
		{
			return CalculateFirstWaveY(x, time) + CalculateSecondWaveY(x, time);

		}

		public float CalculateFirstWaveY(float x, float time)
		{
			//y1=A*sin⁡(t/b-x)
			return Wave.Amplitude * (float)Math.Sin(time / B - x);
		}

		public float CalculateSecondWaveY(float x, float time)
		{
			//y2=A*sin⁡(t/b+x-a)
			return Wave.Amplitude * (float)Math.Sin(time / B + x - (int)Wave.A);
		}
	}
}
