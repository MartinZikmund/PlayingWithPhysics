using System;
using System.Linq;

namespace Physics.WaveInterference.Logic
{
	public class CompoundOscillationsPhysicsService : IOscillationPhysicsService
	{
		private readonly OscillationPhysicsService[] _oscillationPhysicsServices;

		public CompoundOscillationsPhysicsService(params OscillationInfo[] oscillations)
		{
			if (oscillations == null)
			{
				throw new ArgumentNullException(nameof(oscillations));
			}

			_oscillationPhysicsServices = oscillations.Select(o => new OscillationPhysicsService(o)).ToArray();
		}

		public float CalculateA(float timeInSeconds)
		{
			if (_oscillationPhysicsServices.Length > 0)
			{
				return _oscillationPhysicsServices.Sum(o => o.CalculateA(timeInSeconds));
			}
			return 0.0f;
		}

		public float CalculateV(float timeInSeconds)
		{
			if (_oscillationPhysicsServices.Length > 0)
			{
				return _oscillationPhysicsServices.Sum(o => o.CalculateV(timeInSeconds));
			}
			return 0.0f;
		}

		public float CalculateY(float timeInSeconds)
		{
			if (_oscillationPhysicsServices.Length > 0)
			{
				return _oscillationPhysicsServices.Sum(o => o.CalculateY(timeInSeconds));
			}
			return 0.0f;
		}
	}
}
