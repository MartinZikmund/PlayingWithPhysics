namespace Physics.HuygensPrinciple.Logic
{
	public class HuygensFieldBuilder
	{
		private HuygensField _currentField;

		public HuygensFieldBuilder(int width, int height)
		{
			_currentField = new HuygensField(width, height);
		}
		
		public void DrawScene(ScenePreset scenePreset)
		{
			if (scenePreset is BitmapScenePreset bitmapPreset)
			{
				_currentField = bitmapPreset.BaseBitmap.Clone();
			}

			foreach (var shape in scenePreset)
			{
				shape.Draw(_currentField);
			}
		}

		public HuygensField Build() => _currentField;
	}
}
