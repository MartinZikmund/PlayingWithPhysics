using System;

namespace Physics.LawOfConservationOfMomentum.Game
{
	public class GameAttemptPhysicsService
	{
		private readonly GamePhysicsService _parentService;
		private readonly float _ballMass;
		private readonly float _baronMass;
		private readonly float _netPeriod;
		private readonly float _ballVelocity;
		private readonly float _fireTime;

		internal GameAttemptPhysicsService(
			GamePhysicsService parentService,
			float ballMass,
			float baronMass,
			float netPeriod,
			float ballVelocity,
			float fireTime)
		{
			_parentService = parentService;
			_ballMass = ballMass;
			_baronMass = baronMass;
			_netPeriod = netPeriod;
			_ballVelocity = ballVelocity;
			_fireTime = fireTime;
		}

		public static float CollisionX => 1.84f;

		private float NetX => 14;

		public float CollisionTime => CollisionX / _ballVelocity;

		public float CalculateBallVelocityAfterCollision() =>
			(_ballMass * _ballVelocity - _baronMass * _ballVelocity) / (_ballMass + _baronMass);

		public float CalculateBaronVelocityAfterCollision() =>
			2 * _ballVelocity * (_ballMass / (_ballMass + _baronMass));

		public float CalculateBallX(float simulationTime)
		{
			var t = simulationTime - _fireTime;
			if (t <= CollisionTime)
			{
				return _ballVelocity * t;
			}
			else
			{
				return CollisionX + CalculateBallVelocityAfterCollision() * (t - CollisionTime);
			}
		}

		public float CalculateBaronX(float simulationTime)
		{
			var t = simulationTime - _fireTime;
			t = Math.Min(t, CalculateEndTime());
			if (t <= CollisionTime)
			{
				return CollisionX;
			}
			else
			{
				return CollisionX + CalculateBaronVelocityAfterCollision() * (t - CollisionTime);
			}
		}

		public float CurrentDistance(float simulationTime)
		{
			var distAbs = Math.Abs(CalculateBaronX(simulationTime) - NetX);
			var y = CalculateNetY(simulationTime);
			return (float)Math.Sqrt(distAbs * distAbs + y * y);
		}

		public float CalculateEndTime() =>
			CollisionX / _ballVelocity +
			(NetX - CollisionX) / CalculateBaronVelocityAfterCollision();

		public float CalculateNetY(float simulationTime)
		{
			var t = simulationTime - _fireTime;
			if (t >= CalculateEndTime())
			{
				var adjustedSimulationTime = CalculateEndTime() + _fireTime;
				return _parentService.CalculateNetY(adjustedSimulationTime);
			}
			else
			{
				return _parentService.CalculateNetY(simulationTime);
			}
		}
	}
}
