using System;
using System.Collections.Generic;

namespace Physics.GravitationalFieldMovement.Logic;

public class PhysicsService
{
	private const int DataPointCount = 30;

	public PhysicsService(InputConfiguration input)
	{
		Input = input;
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

	private double Dt => Input.T / DataPointCount;

	private TrajectoryPoint[] CalculateEllipseTrajectory()
	{
		var results = new List<TrajectoryPoint>();
		for (int dataPointIndex = 0; dataPointIndex < DataPointCount; dataPointIndex++)
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

			var theta = 2 * Math.Atan2(Math.Sqrt(1 + Input.Eps) * Math.Sin(E / 2), Math.Sqrt(1 - Input.Eps) * Math.Cos(E / 2));
			var r = Input.P / (1 + Input.Eps * Math.Cos(theta));
			var phi = Input.Omega + Input.SigL * theta;
			var x = r * Math.Cos(phi);
			var y = r * Math.Sin(phi);
			var v = Math.Sqrt(2 * (Input.En + Input.Alpha / r));
			var trajectoryPoint = new TrajectoryPoint(t, x, y, v);
			results.Add(trajectoryPoint);
		}
		return results.ToArray();
	}

	private TrajectoryPoint[] CalculateHyperbolaTrajectory()
	{
		return Array.Empty<TrajectoryPoint>();
	}
}
