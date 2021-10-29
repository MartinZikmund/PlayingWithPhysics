using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class Range
	{
		public Range(float min, float max)
		{
			Min = min;
			Max = max;
		}
		public float Min { get; set; }
		public float Max { get; set; }
		public string Label => ToString();
		public override string ToString() => $"{Min}-{Max}π";
	}
}
