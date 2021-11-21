﻿using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public static class ScenePresets
	{
		public static ScenePreset[] EasyVariant { get; } = new ScenePreset[]
		{
			new ScenePreset("Point")
			{
				new Circle(new PointF(0.5f, 0.5f), 0.002f)
			},
			new ScenePreset("Square")
			{
				new Rectangle(new PointF(0.48f,0.48f), new PointF(0.52f, 0.52f))
			},
			new ScenePreset("Brick")
			{
				new Rectangle(new PointF(0.44f,0.48f), new PointF(0.56f, 0.52f))
			},
			new ScenePreset("Circle")
			{
				new Circle(new PointF(0.5f, 0.5f), 0.05f)				
			},
			new ScenePreset("Line")
			{
				new Rectangle(new PointF(0,0), new PointF(1,0.01f))
			},
			new ScenePreset("Snowman")
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
				new Rectangle(new PointF(0,0.5f), new PointF(0.45f, 0.55f), CellState.Wall),
				new Rectangle(new PointF(0.55f,0.5f), new PointF(1f, 0.55f), CellState.Wall),
			},
			new ScenePreset("LineWall")
			{
				new Rectangle(new PointF(0,0), new PointF(1,0.01f)),
				new Rectangle(new PointF(0,0.5f), new PointF(0.45f, 0.55f), CellState.Wall),
				new Rectangle(new PointF(0.55f,0.5f), new PointF(1f, 0.55f), CellState.Wall),
			},
		};
	}
}