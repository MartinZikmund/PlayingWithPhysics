namespace Physics.CompoundOscillations.Logic
{
	public class OscillationInfo
    {
		public OscillationInfo(string label, double amplitude, double frequence, double phase, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequency = frequence;
			Phase = phase;
			Color = color;
		}

		public string Label { get; set; }

        public double Amplitude { get; set; }

		public double Frequency { get; set; }

		public double Phase { get; set; }

		public string Color { get; set; }

		public OscillationInfo Clone() => new OscillationInfo(Label, Amplitude, Frequency, Phase, Color);
	}
}
