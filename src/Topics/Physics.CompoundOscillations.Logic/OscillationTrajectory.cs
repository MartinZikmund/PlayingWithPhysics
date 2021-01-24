using System;

namespace Physics.CompoundOscillations.Logic
{
	public class OscillationTrajectory : IOscillationTrajectory
    {
		private readonly float _period;
		private readonly OscillationPoint[] _points;

		public OscillationTrajectory(float period, params OscillationPoint[] points)
		{
			if (points.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(points));
			_points = points ?? throw new ArgumentNullException(nameof(points));
			_period = period;
		}

		public float GetY(float timeInSeconds)
		{
			if (timeInSeconds < 0)
			{
				return float.NaN;
			}

			var timeInPeriod = timeInSeconds % _period;
			//TODO: Binary search
			int lowerPointId = 0;
			while(lowerPointId < _points.Length && _points[lowerPointId].Time <= timeInPeriod)
			{
				lowerPointId++;
			}
			if(lowerPointId == _points.Length)
			{
				lowerPointId--;
			}
			return _points[lowerPointId].Y;
		}
	}
}
