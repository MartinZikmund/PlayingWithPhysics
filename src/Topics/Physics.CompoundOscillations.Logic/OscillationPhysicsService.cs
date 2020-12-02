using System;

namespace Physics.CompoundOscillations.Logic
{
	public class OscillationPhysicsService : IOscillationPhysicsService
	{
		private readonly OscillationInfo _oscillationInfo;

		public OscillationPhysicsService(OscillationInfo oscillationInfo)
		{
			_oscillationInfo = oscillationInfo;
		}

		public double CalculateY(double timeInSeconds) =>
			_oscillationInfo.Amplitude * Math.Sin(2 * Math.PI * timeInSeconds * _oscillationInfo.Frequency + _oscillationInfo.Phase);

		public double CalculatePeriod() => 1 / _oscillationInfo.Frequency;
	}
}
