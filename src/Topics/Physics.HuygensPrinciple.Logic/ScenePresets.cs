using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public static class ScenePresets
	{
		public static ScenePreset[] Presets { get; } = new[]
		{
			new ScenePreset("Point")
			{
				new Circle(new PointF(0.5f, 0.5f), 0.005f)
			},
			new ScenePreset("Square")
			{
				new Circle(new PointF(0.5f, 0.5f), 0.05f)
			},
			new ScenePreset("Circle")
			{
				new Rectangle(new PointF(0.48f,0.48f), new PointF(0.52f, 0.52f))
			},
			new ScenePreset("Line")
			{
				new Rectangle(new PointF(0,0), new PointF(1,0.01f))
			},
			new ScenePreset("Wall")
			{
				new Circle(new PointF(0.5f, 0.02f), 0.01f),
				new Rectangle(new PointF(0,0.5f), new PointF(0.45f, 0.55f), CellState.Wall),
				new Rectangle(new PointF(0.55f,0.5f), new PointF(1f, 0.55f), CellState.Wall),
			}
		};
	}
}
