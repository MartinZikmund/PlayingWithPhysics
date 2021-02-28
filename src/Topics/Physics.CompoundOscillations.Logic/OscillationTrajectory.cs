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
			int lowerPointId = BinarySearch(timeInPeriod);

			if (accurate)
			{
				return CalculateMidpoint(timeInPeriod, lowerPointId);
			}
			else
			{
				return _points[lowerPointId].Y;
			}
		}


		private int BinarySearch(float timeInPeriod)
		{
			var minIndex = 0;
			var maxIndex = _points.Length - 1;
			var currentIndex = _points.Length / 2;

			while (true)
			{
				if (_points[currentIndex].Time > timeInPeriod)
				{
					maxIndex = currentIndex - 1;
				}
				else if (_points[currentIndex].Time < timeInPeriod)
				{
					minIndex = currentIndex + 1;
				}
				if (_points[currentIndex].Time == timeInPeriod)
				{
					return currentIndex;
				}

				if (minIndex > maxIndex || currentIndex < 0 || currentIndex >= _points.Length)
				{
					break;
				}

				currentIndex = (minIndex + maxIndex) / 2;
			}

			// Find lowest higher
			if (currentIndex < 0)
			{
				currentIndex = 0;
			}

			while (currentIndex < _points.Length - 1 && _points[currentIndex + 1].Time <= timeInPeriod)
			{
				currentIndex++;
			}

			if (currentIndex >= _points.Length)
			{
				currentIndex = _points.Length - 1;
			}

			return currentIndex;
		}



		private float CalculateMidpoint(float time, int index)
		{
			if (time < _points[index].Time && index > 0)
			{
				index--;
			}
			var lowerPoint = _points[index];
			OscillationPoint upperPoint;
			if (index == _points.Length - 1)
			{
				upperPoint = _points[index];
			}
			else
			{
				upperPoint = _points[index + 1];
			}
			var lowerTime = lowerPoint.Time;
			var upperTime = upperPoint.Time;
			var spanTime = upperTime - lowerTime;
			var relativeTime = time - lowerTime;

			var ratio = relativeTime / spanTime;
			var valueDiff = upperPoint.Y - lowerPoint.Y;
			return lowerPoint.Y + (float)(valueDiff * ratio);
		}
	}
}
