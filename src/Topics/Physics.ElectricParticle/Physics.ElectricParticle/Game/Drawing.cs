using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Physics.ElectricParticle.Game
{
	public class Drawing
    {
		public List<DrawingPath> _drawingPaths = new List<DrawingPath>();

		public IReadOnlyList<DrawingPath> Paths => _drawingPaths;

		public void AddPoint(Vector2 point)
		{
			PenDown();
			_drawingPaths.Last().AddPoint(point);
		}

		public void PenDown()
		{
			var lastPath = _drawingPaths.LastOrDefault();
			if (lastPath == null || lastPath.IsClosed)
			{
				_drawingPaths.Add(new DrawingPath());
			}
		}

		public void PenUp()
		{
			var lastPath = _drawingPaths.LastOrDefault();
			if (lastPath != null && !lastPath.IsClosed)
			{
				lastPath.Close();
			}
		}
    }
}
