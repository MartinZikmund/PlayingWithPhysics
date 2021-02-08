using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.Helpers;

namespace Physics.LissajousCurves.Logic
{
	public class OscillationInfo
	{
		public OscillationInfo(string label, float amplitude, float frequence, float phase, string color)
		{
			Label = label;
			Amplitude = amplitude;
			Frequency = frequence;
			PhaseInDeg = phase;
			Color = color;
		}

		public string Label { get; set; }

		public float Amplitude { get; set; }

		public float Frequency { get; set; }

		public float PhaseInRad => MathHelpers.DegreesToRadians(PhaseInDeg);

		public float PhaseInDeg { get; set; }

		public string Color { get; set; }

		public OscillationInfo Clone() => new OscillationInfo(Label, Amplitude, Frequency, PhaseInRad, Color);
		public bool IsVisible { get; set; } = true;
	}
}
