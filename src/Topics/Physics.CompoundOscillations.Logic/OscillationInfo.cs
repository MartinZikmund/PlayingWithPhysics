namespace Physics.CompoundOscillations.Logic
{
	public class OscillationInfo
    {
		public OscillationInfo(string label, double amplitude, double frequence, double epsilon, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequence = frequence;
			Epsilon = epsilon;
			Color = color;
		}

		public string Label { get; set; }

        public double Amplitude { get; set; }

		public double Frequence { get; set; }

		public double Epsilon { get; set; }

		public string Color { get; set; }
    }
}
