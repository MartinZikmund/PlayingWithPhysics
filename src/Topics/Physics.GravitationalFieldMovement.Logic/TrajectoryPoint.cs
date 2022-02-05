namespace Physics.GravitationalFieldMovement.Logic;

public class TrajectoryPoint
{
	public TrajectoryPoint(double time, double x, double y, double v)
	{
		Time = time;
		X = x;
		Y = y;
		V = v;
	}

	public double Time { get; set; }

	public double X { get; }

	public double Y { get; }

	public double V { get; }
}
