using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Physics.ElectricParticle.Game
{
	public class DrawingPath
	{
		private List<Vector2> _points = new List<Vector2>();
		private bool _isClosed = false;

		public IReadOnlyList<Vector2> Points => _points;

		public bool IsClosed => _isClosed;

		public void AddPoint(Vector2 point)
		{
			var lastPoint = _points.LastOrDefault();
			if (lastPoint != null)
			{
				var deltaX = point.X - lastPoint.X;
				var deltaY = point.Y - lastPoint.Y;
				if ((deltaX * deltaX + deltaY * deltaY) < 0.0001f)
				{
					return;
				}
			}
			_points.Add(point);
		}

		public void Close() => _isClosed = true;
	}
}
