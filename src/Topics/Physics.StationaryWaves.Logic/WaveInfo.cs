using System;

namespace Physics.StationaryWaves.Logic
{
	public class WaveInfo
	{
		public WaveInfo(float amplitude = 0)
		{
			Amplitude = amplitude;

		}

		public float Amplitude { get; set; }

		public float T { get; set; }

		public float WaveLength { get; set; }
	}
}
