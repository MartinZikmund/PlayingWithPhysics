using System;

namespace Physics.HuygensPrinciple.Logic
{
	public class HuygensFieldBuilder
	{
		private readonly HuygensField _currentField;

		public HuygensFieldBuilder(int width, int height)
		{
			_currentField = new HuygensField(width, height);
		}

		public void DrawScene(ScenePreset scenePreset)
		{
			foreach (var shape in scenePreset)
			{
				shape.Draw(_currentField);
			}
		}

		public HuygensField Build() => _currentField;
	}
}
