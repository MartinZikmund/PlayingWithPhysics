using System;

namespace Physics.CompoundOscillations.Logic
{
	public class OscillationPoint
    {
		public OscillationPoint(TimeSpan time, float y)
		{
			Time = time;
			Y = y;
		}

		/// <summary>
		/// Time at point.
		/// </summary>
		public TimeSpan Time { get; set; }

		/// <summary>
		/// Y in meters.
		/// </summary>
		public float Y { get; set; }
	}
}
