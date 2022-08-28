using System;

namespace Physics.LawOfConservationOfMomentum.Game
{
	public class GamePhysicsService
	{
		private readonly float _ballMass;
		private readonly float _baronMass;
		private readonly float _netPeriod;

		public GamePhysicsService(
			float ballMass,
			float baronMass,
			float netPeriod)
		{
			_ballMass = ballMass;
			_baronMass = baronMass;
			_netPeriod = netPeriod;
		}

		private float NetA => 140;

		public GameAttemptPhysicsService StartAttempt(float initialVelocity, float fireTime)
		{
			return new GameAttemptPhysicsService(
				this,
				_ballMass,
				_baronMass,
				_netPeriod,
				initialVelocity,
				fireTime);
		}

		public float CalculateNetY(float simulationTime)
		{
			return (float)(NetA * Math.Sin(2 * Math.PI * simulationTime / _netPeriod));
		}
	}
}
