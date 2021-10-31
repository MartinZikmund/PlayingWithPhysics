using System.Collections;
using System.Collections.Generic;

namespace Physics.HuygensPrinciple.Logic
{
	public class ScenePreset : IEnumerable<IShape>
	{
		private readonly List<IShape> _shapes = new List<IShape>();

		public ScenePreset(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public void Render(HuygensStepper stepper)
		{
			foreach (var shape in _shapes)
			{
				shape.Render(stepper);
			}
		}

		public void Add(IShape shape) => _shapes.Add(shape);

		public IEnumerator<IShape> GetEnumerator() => _shapes.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
