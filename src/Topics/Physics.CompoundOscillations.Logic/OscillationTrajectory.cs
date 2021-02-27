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

		public float GetY(float timeInSeconds, bool accurate = false)
		{
			if (timeInSeconds < 0)
			{
				return float.NaN;
			}

			var timeInPeriod = timeInSeconds % _period;
			//TODO: Binary search
			int lowerPointId = 0;
			while(lowerPointId < _points.Length - 1 && _points[lowerPointId + 1].Time <= timeInPeriod)
			{
				lowerPointId++;
			}
			if(lowerPointId == _points.Length)
			{
				lowerPointId--;
			}

			if (accurate)
			{
				return CalculateMidpoint(timeInPeriod, lowerPointId);
			}
			else
			{
				return _points[lowerPointId].Y;
			}
		}

		private float CalculateMidpoint(float time, int index)
		{
			if (index == _points.Length - 1)
			{
				return _points[index].Y;
			}

			var lowerTime = _points[index].Time;
			var upperTime = _points[index + 1].Time;
			var spanTime = upperTime - lowerTime;
			var relativeTime = time - lowerTime;

			var ratio = relativeTime / spanTime;
			var valueDiff = _points[index + 1].Y - _points[index].Y;
			return _points[index].Y + (float)(valueDiff * ratio);
		}
	}
}
