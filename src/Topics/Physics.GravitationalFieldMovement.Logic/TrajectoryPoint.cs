namespace Physics.GravitationalFieldMovement.Logic;

public class TrajectoryPoint
{
	public TrajectoryPoint(double time, double x, double y, double phi, double r, double h, double v)
	{
		Time = time;
		X = x;
		Y = y;
		Phi = phi;
		R = r;
		H = h;
		V = v;
	}

	public double Time { get; set; }

	public double X { get; }

	public double Y { get; }

	public double Phi { get; }

	public double R { get; }

	public double H { get; }

	public double V { get; }
}
