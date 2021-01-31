using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.LissajousCurves.Logic
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

		public float PhaseInPiRad => (float)(PhaseInRad / Math.PI);

		public string Color { get; set; }

		public OscillationInfo Clone() => new OscillationInfo(Label, Amplitude, Frequency, PhaseInRad, Color);
	}
}
