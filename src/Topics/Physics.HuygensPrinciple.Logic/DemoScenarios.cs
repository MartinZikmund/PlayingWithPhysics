using System.Drawing;

namespace Physics.HuygensPrinciple.Logic;

public static class DemoScenarios
{
	public static DemoScenario[] Scenarios { get; } = new DemoScenario[]
	{
		new ("Krakatoa", new Circle(new PointF(0.82f,0.556f), 0.01f)),
		new ("Vesuvius", new Circle(new PointF(0.63f,0.42f), 0.01f)),
		new ("Chicxulub", new Circle(new PointF(0.3f,0.41f), 0.01f)),
		new ("Santorini", new Circle(new PointF(0.52f,0.48f), 0.01f)),
		new ("Japan", new Circle(new PointF(0.76f,0.49f), 0.01f)),
	};
}
