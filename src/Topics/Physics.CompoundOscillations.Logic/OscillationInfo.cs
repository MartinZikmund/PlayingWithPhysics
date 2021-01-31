using System;

namespace Physics.CompoundOscillations.Logic
{
	public class OscillationInfo
	{
		public OscillationInfo(string label, float amplitude, float frequence, float phase, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequency = frequence;
			PhaseInRad = phase;
			Color = color;
		}

		public string Label { get; set; }

		public float Amplitude { get; set; }

		public string AmplitudeText => Amplitude.ToString("0.##");

		public float Frequency { get; set; }

		public string FrequencyText => Frequency.ToString("0.##");

		public float PhaseInRad { get; set; }

		public string Color { get; set; }

		public string PhaseInPiRadText => (PhaseInRad / Math.PI).ToString("0.###");

		public OscillationInfo Clone() => new OscillationInfo(Label, Amplitude, Frequency, PhaseInRad, Color);
	}
}
