namespace Physics.CompoundOscillations.Logic
{
	public class OscillationInfo
    {
		public OscillationInfo(string label, float amplitude, float frequence, float phase, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequency = frequence;
			Phase = phase;
			Color = color;
		}

		public string Label { get; set; }

        public float Amplitude { get; set; }

		public float Frequency { get; set; }

		public float Phase { get; set; }

		public string Color { get; set; }

		public OscillationInfo Clone() => new OscillationInfo(Label, Amplitude, Frequency, Phase, Color);
	}
}
