using System;

namespace Physics.CompoundOscillations.Logic
{
	public class OscillationTrajectory
    {
		private readonly double _period;
		private readonly OscillationPoint[] _points;

		public OscillationTrajectory(double period, params OscillationPoint[] points)
		{
			if (points.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(points));
			_points = points ?? throw new ArgumentNullException(nameof(points));
			_period = period;
		}

		public double GetY(double timeInSeconds)
		{
			//TODO: Get point based on period
			return 0.0;
		}
	}
}
