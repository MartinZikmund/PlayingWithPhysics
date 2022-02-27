using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Physics.Shared.Logic.Geometry;

namespace Physics.HuygensPrinciple.Logic
{
	public class ScenePreset : IEnumerable<IShape>
	{
		private readonly List<IShape> _shapes = new List<IShape>();

		public ScenePreset(string name, PointF[] significantPoints = null)
		{
			Name = name;
			SignificantPoints = significantPoints ?? Array.Empty<PointF>();
		}

		public string Name { get; }

		public PointF[] SignificantPoints { get; }

		public void Add(IShape shape) => _shapes.Add(shape);

		public void Remove(IShape shape) => _shapes.Remove(shape);

		public IEnumerator<IShape> GetEnumerator() => _shapes.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool Redrawn { get; set; } = false;

		public ScenePreset Clone()
		{
			var preset = new ScenePreset(Name, SignificantPoints.ToArray());
			preset._shapes.AddRange(_shapes);
			return preset;
		}

		public void EraseAt(double relativeX, double relativeY)
		{
			for (int i = _shapes.Count - 1; i >= 0; i--)
			{
				var shape = _shapes[i];
				if (shape.HitTest(relativeX, relativeY))
				{
					Remove(shape);
				}

			}
		}
	}
}
