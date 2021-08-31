using System;

namespace Physics.WaveInterference.Logic
{
	public class WaveInfo
	{
		public WaveInfo(string label, float amplitude, float frequency, float waveLength, float phase, WaveDirection direction, float sourceDistance, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequency = frequency;
			WaveLength = waveLength;
			PhaseInRad = phase;
			Direction = direction;
			SourceDistance = sourceDistance;
			Color = color;
		}

		public float SourceDistance { get; set; }
		public string Label { get; set; }
		public float Amplitude { get; set; }
		public string AmplitudeText => Amplitude.ToString("0.##");
		public float Frequency { get; set; }
		public string FrequencyText => Frequency.ToString("0.##");
		public float WaveLength { get; set; }
		public string WaveLengthText => WaveLength.ToString("0.##");
		//public float PhaseInRad { get; set; }
		//public string PhaseInPiRadText => (PhaseInRad / Math.PI).ToString("0.###");
		public float Period => 1.0f / Frequency;
		public string PeriodText => Period.ToString("0.###");
		public WaveDirection Direction { get; set; }

		public string Color { get; set; }
		public WaveInfo Clone() => new WaveInfo(Label, Amplitude, Frequency, WaveLength, PhaseInRad, Direction, SourceDistance, Color);
	}
}
