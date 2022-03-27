using System;
using System.Collections.Generic;

namespace Physics.GravitationalFieldMovement.Logic;

public class PhysicsService
{
	private const int DataPointCount = 30;

	private readonly double _dt;

	public PhysicsService(InputConfiguration input, double dt)
	{
		Input = input;
		_dt = dt;
	}

	public InputConfiguration Input { get; }

	public TrajectoryPoint[] CalculateTrajectory()
	{
		if (Input.ConicSec == MovementType.Ellipse)
		{
			return CalculateEllipseTrajectory();
		}
		else if (Input.ConicSec == MovementType.Hyperbola)
		{
			return CalculateHyperbolaTrajectory();
		}
		else
		{
			return Array.Empty<TrajectoryPoint>();
		}
	}

	private double Dt => _dt; //TODO Input.T / DataPointCount;

	private TrajectoryPoint[] CalculateEllipseTrajectory()
	{
		var results = new List<TrajectoryPoint>();
		var impact = false;
		for (int dataPointIndex = 0; dataPointIndex < DataPointCount && !impact; dataPointIndex++)
		{
			var t = Dt * dataPointIndex;
			var M = Input.N * (t - Input.Tau);
			var E =
				Math.Sign(M) *
				Math.Min(
					Math.Min(
						Math.Abs(M) / (1 - Input.Eps),
						Math.Abs(M) + Input.Eps),
					(Math.Abs(M) + Math.PI * Input.Eps) / (1 + Input.Eps));

			for (int i = 0; i < 5; i++)
			{
				E = (M - Input.Eps * (E * Math.Cos(E) - Math.Sin(E))) / (1 - Input.Eps * Math.Cos(E));
			}

			var theta = 2 * Math.Atan2(
				Math.Sqrt(1 + Input.Eps) * Math.Sin(E / 2),
				Math.Sqrt(1 - Input.Eps) * Math.Cos(E / 2));
			var r = Input.P / (1 + Input.Eps * Math.Cos(theta));
			var phi = Input.Omega + Input.SigL * theta;
			var x = r * Math.Cos(phi);
			var y = r * Math.Sin(phi);
			var v = Math.Sqrt(2 * (Input.En + Input.Alpha / r));
			var h = y - Input.Rz;
			var trajectoryPoint = new TrajectoryPoint(t, x, y, v, h);
			results.Add(trajectoryPoint);

			impact = r < Input.Rz;
		}
		return results.ToArray();
	}

	private TrajectoryPoint[] CalculateHyperbolaTrajectory()
	{
		var results = new List<TrajectoryPoint>();
		var a1 = 0.368;
		var c1 = 1.543;
		var a8 = 10433;
		var c8 = 1490;
		var impact = false;
		for (int dataPointIndex = 0; dataPointIndex < DataPointCount && !impact; dataPointIndex++)
		{
			var t = Dt * dataPointIndex;
			var M = Input.N * (t - Input.Tau);
			var absMe1 = Math.Abs(M) / (Input.Eps - 1);
			var absMA1 = (Math.Abs(M) + a1 * Input.Eps) / (c1 * Input.Eps - 1);
			var absMA8 = (Math.Abs(M) + a8 * Input.Eps) / (c8 * Input.Eps - 1);
			var H = M * Math.Min(absMe1, Math.Min(absMA1, absMA8));

			for (int i = 0; i < 8; i++)
			{
				H = (M + Input.Eps * (H * Math.Cosh(H) - Math.Sinh(H))) / (Input.Eps * Math.Cosh(H) - 1);
			}

			var theta = 2 * Math.Atan2(Math.Sqrt(Input.Eps + 1) * Math.Sinh(H / 2), Math.Sqrt(Input.Eps - 1) * Math.Cosh(H / 2));
			var r = Input.P / (1 + Input.Eps * Math.Cos(theta));
			var phi = Input.Omega + Input.SigL * theta;
			var x = r * Math.Cos(phi);
			var y = r * Math.Sin(phi);
			var v = Math.Sqrt(2 * (Input.En + Input.Alpha / r));
			var h = y - Input.Rz;
			var trajectoryPoint = new TrajectoryPoint(t, x, y, v, h);
			results.Add(trajectoryPoint);

			impact = r < Input.Rz;
		}
		return results.ToArray();
	}
}
