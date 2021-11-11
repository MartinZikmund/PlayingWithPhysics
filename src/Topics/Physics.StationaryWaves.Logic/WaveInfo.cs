using System;

namespace Physics.StationaryWaves.Logic
{
	public class WaveInfo
	{
		public WaveInfo(string label, BouncingPoints selectedBouncingPoint, float amplitude = 0, AVariant a = AVariant.Zero, Range range = null, string color = "#FF0000")
		{
			SelectedBouncingPoint = selectedBouncingPoint;
			Label = label;
			Amplitude = amplitude;
			A = a;
			DrawingRange = range;
			Color = color;
		}

		public BouncingPoints SelectedBouncingPoint { get; set; }

		public string Label { get; set; }
		public float Amplitude { get; set; }
		public string AmplitudeText => Amplitude.ToString("0.##");

		public AVariant A { get; set; }

		public Range DrawingRange { get; set; }
		public string DrawingRangeText => DrawingRange.ToString();
		public string Color { get; set; }
		public WaveInfo Clone() => new WaveInfo(Label, SelectedBouncingPoint, Amplitude, A, DrawingRange, Color);
	}
}
