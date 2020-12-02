namespace Physics.CompoundOscillations.Logic
{
	public class OscillationInfo
    {
		public OscillationInfo(string label, double amplitude, double frequence, double epsilon, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequency = frequence;
			Phase = epsilon;
			Color = color;
		}

		public string Label { get; set; }

        public double Amplitude { get; set; }

		public double Frequency { get; set; }

		public double Phase { get; set; }

		public string Color { get; set; }
    }
}
