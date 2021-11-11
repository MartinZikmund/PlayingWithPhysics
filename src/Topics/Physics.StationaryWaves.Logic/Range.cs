using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.StationaryWaves.Logic
{
	public class Range
	{
		public Range(float min, float maxFloat)
		{
			Min = min;
			MaxFloat = maxFloat;
		}
		public float Min { get; set; }
		private float MaxFloat { get; set; }
		public float Max => MaxFloat * (float)Math.PI;
		public string Label => ToString();
		public override string ToString() => $"{Min}-{MaxFloat}π";
	}
}
