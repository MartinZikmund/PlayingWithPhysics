using System;
using System.Collections.Generic;

namespace Physics.LissajousCurves.Logic
{
	public class OscillationPhysicsService : IOscillationPhysicsService
	{
		private readonly OscillationInfo _oscillationInfo;

		public OscillationPhysicsService(OscillationInfo oscillationInfo)
		{
			_oscillationInfo = oscillationInfo;
		}

		public float GetTimeAtDistance(float distance) =>
			(distance - _oscillationInfo.PhaseInRad) / (2 * (float)Math.PI * _oscillationInfo.Frequency);

		public float CalculateDistance(float timeInSeconds) =>
			2 * (float)Math.PI * timeInSeconds * _oscillationInfo.Frequency + _oscillationInfo.PhaseInRad;

		public float CalculateY(float timeInSeconds) =>
			_oscillationInfo.Amplitude * (float)Math.Sin(2 * (float)Math.PI * timeInSeconds * _oscillationInfo.Frequency + _oscillationInfo.PhaseInRad);

		public float CalculateA(float timeInSeconds) =>
			-_oscillationInfo.Amplitude *
			(2 * (float)Math.PI * _oscillationInfo.Frequency) *
			(2 * (float)Math.PI * _oscillationInfo.Frequency) *
			(float)Math.Sin(2 * Math.PI * timeInSeconds * _oscillationInfo.Frequency + _oscillationInfo.PhaseInRad);

		public float CalculateV(float timeInSeconds) =>
			_oscillationInfo.Amplitude *
			(2 * (float)Math.PI * _oscillationInfo.Frequency) *
			(float)Math.Cos(2 * Math.PI * timeInSeconds * _oscillationInfo.Frequency + _oscillationInfo.PhaseInRad);

		public float CalculatePeriod() => 1 / _oscillationInfo.Frequency;

		public float MaxY => Math.Abs(_oscillationInfo.Amplitude);

		public float MinY => -Math.Abs(_oscillationInfo.Amplitude);

		public OscillationTrajectory CreateTrajectoryData()
		{
			var period = CalculatePeriod();
			if (Math.Abs(period) < 0.00001)
			{
				return new OscillationTrajectory(period, new OscillationPoint(TimeSpan.Zero, CalculateY(0)));
			}
			var frameTime = 1 / 400f;
			var jumpCount = (int)Math.Ceiling(period / frameTime);
			//TODO?
			//if (jumpCount > MaxTrajectoryJumps)
			//{
			//	jumpCount = MaxTrajectoryJumps;
			//}

			//if (jumpCount < MinTrajectoryJumps)
			//{
			//	jumpCount = MinTrajectoryJumps;
			//}
			var jumpSize = period / jumpCount;
			var currentTime = 0.0f;
			var points = new List<OscillationPoint>();
			while (currentTime <= period)
			{
				points.Add(new OscillationPoint(TimeSpan.FromSeconds(currentTime), CalculateY(currentTime)));
				currentTime += jumpSize;
			}
			points.Add(new OscillationPoint(TimeSpan.FromSeconds(period), CalculateY(period)));
			return new OscillationTrajectory(period, points.ToArray());
		}
	}
}
