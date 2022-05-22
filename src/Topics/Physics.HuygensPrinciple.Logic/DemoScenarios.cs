using System.Drawing;

namespace Physics.HuygensPrinciple.Logic;

public static class DemoScenarios
{
	public static DemoScenario[] Scenarios { get; } = new DemoScenario[]
	{
		new ("Krakatoa", new Circle(new PointF(0.5f,0.5f), 2)),
		new ("Vesuvius", new Circle(new PointF(0.5f,0.5f), 2)),
		new ("Chicxulub", new Circle(new PointF(0.5f,0.5f), 2)),
		new ("Santorini", new Circle(new PointF(0.5f,0.5f), 2)),
		new ("Japan", new Circle(new PointF(0.5f,0.5f), 2)),
	};
}
