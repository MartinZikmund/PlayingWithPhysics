using System;
using System.Linq;

namespace Physics.CompoundOscillations.Logic
{
	public class CompoundOscillationsPhysicsService
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

		public double CalculateY(double timeInSeconds)
		{
			if (_oscillationPhysicsServices.Length > 0)
			{
				return _oscillationPhysicsServices.Sum(o => o.CalculateY(timeInSeconds));
			}
			return 0.0;
		}
	}
}
