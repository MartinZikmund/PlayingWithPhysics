using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public static class ScenePresets
	{
		public static ScenePreset[] Presets { get; } = new[]
		{			
			new ScenePreset("Ball")
			{
				new Rectangle(new PointF(0.4f,0.4f), new PointF(0.6f, 0.6f))
			},
			new ScenePreset("Brick")
			{
				new Circle(new PointF(0.5f, 0.5f), 0.2f)
			},
		};
	}
}
