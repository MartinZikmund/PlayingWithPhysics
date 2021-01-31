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

		public float Frequency { get; set; }

		public float PhaseInRad { get; set; }

		public string Color { get; set; }

		public float PhaseInPiRad => (float)(PhaseInRad / Math.PI);

		public OscillationInfo Clone() => new OscillationInfo(Label, Amplitude, Frequency, PhaseInRad, Color);
	}
}
