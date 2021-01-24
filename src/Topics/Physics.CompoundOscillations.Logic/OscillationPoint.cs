using System;

namespace Physics.CompoundOscillations.Logic
{
	public class OscillationPoint
    {
		public OscillationPoint(double time, float y)
		{
			Time = time;
			Y = y;
		}

		/// <summary>
		/// Time at point.
		/// </summary>
		public double Time { get; set; }

		/// <summary>
		/// Y in meters.
		/// </summary>
		public float Y { get; set; }
	}
}
