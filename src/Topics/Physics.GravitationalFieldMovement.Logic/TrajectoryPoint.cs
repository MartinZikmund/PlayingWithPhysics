namespace Physics.GravitationalFieldMovement.Logic;

public class TrajectoryPoint
{
	public TrajectoryPoint(double time, double x, double y, double v, double h)
	{
		Time = time;
		X = x;
		Y = y;
		V = v;
		H = h;
	}

	public double Time { get; set; }

	public double X { get; }

	public double Y { get; }

	public double V { get; }

	public double H { get; }
}
