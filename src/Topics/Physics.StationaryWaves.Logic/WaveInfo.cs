using System;

namespace Physics.StationaryWaves.Logic
{
	public class WaveInfo
	{
		public WaveInfo(string label, float amplitude, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Color = color;
		}

		public string Label { get; set; }
		public float Amplitude { get; set; }
		public string AmplitudeText => Amplitude.ToString("0.##");
		public string Color { get; set; }
		public WaveInfo Clone() => new WaveInfo(Label, Amplitude, Color);
	}
}
