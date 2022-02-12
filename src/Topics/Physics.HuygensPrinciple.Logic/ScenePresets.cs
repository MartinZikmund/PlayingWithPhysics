using System.Drawing;
using Physics.Shared.Logic.Geometry;

namespace Physics.HuygensPrinciple.Logic
{
	public static class ScenePresets
	{
		public static ScenePreset[] EasyVariant { get; } = new ScenePreset[]
		{
			new ScenePreset("Point", new []{new PointF(0.5f, 0.5f) })
			{
				new Circle(new PointF(0.5f, 0.5f), 0.00001f)
			},
			new ScenePreset("Square", new []{new PointF(0.48f, 0.48f), new PointF(0.48f, 0.52f), new PointF(0.52f, 0.48f), new PointF(0.52f, 0.52f)})
			{
				new Rectangle(new PointF(0.48f,0.48f), new PointF(0.52f, 0.52f))
			},
			new ScenePreset("Brick", new []{new PointF(0.44f, 0.48f), new PointF(0.44f, 0.52f), new PointF(0.56f, 0.48f), new PointF(0.56f, 0.52f)})
			{
				new Rectangle(new PointF(0.44f,0.48f), new PointF(0.56f, 0.52f))
			},
			new ScenePreset("Circle", new []{new PointF(0.45f, 0.5f), new PointF(0.55f, 0.5f), new PointF(0.5f, 0.45f), new PointF(0.5f, 0.55f)})
			{
				new Circle(new PointF(0.5f, 0.5f), 0.05f)
			},
			new ScenePreset("Line", new []{new PointF(0.25f, 0.01f), new PointF(0.5f, 0.01f), new PointF(0.75f, 0.01f)})
			{
				new Rectangle(new PointF(0,0), new PointF(1,0.01f))
			},
			new ScenePreset("Snowman", new []{new PointF(0.42f, 0.6f), new PointF(0.56f, 0.5f), new PointF(0.46f, 0.42f)})
			{
				new Circle(new PointF(0.5f, 0.42f), 0.04f),
				new Circle(new PointF(0.5f, 0.5f), 0.06f),
				new Circle(new PointF(0.5f, 0.6f), 0.08f)
			},
		};

		public static ScenePreset[] AdvancedVariant { get; } = new[]
		{
			new ScenePreset("PointWall")
			{
				new Circle(new PointF(0.5f, 0.02f), 0.01f),
				new Rectangle(new PointF(0,0.5f), new PointF(0.45f, 0.6f), CellState.Wall),
				new Rectangle(new PointF(0.55f,0.5f), new PointF(1f, 0.6f), CellState.Wall),
			},
			new ScenePreset("LineWall")
			{
				new Rectangle(new PointF(0,0), new PointF(1,0.01f)),
				new Rectangle(new PointF(0,0.5f), new PointF(0.45f, 0.6f), CellState.Wall),
				new Rectangle(new PointF(0.55f,0.5f), new PointF(1f, 0.6f), CellState.Wall),
			},
		};
	}
}
