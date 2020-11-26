using Physics.DragMovement.Gamification;

namespace Physics.DragMovement.Logic.PhysicsServices
{
	public class RaftPhysicsService
    {
		private const int StartX = 50;
		private const int EndX = 220;

		private readonly GameInfo _gameInfo;

		public RaftPhysicsService(GameInfo gameInfo)
		{
			_gameInfo = gameInfo;
		}

		public double GetX(double time)
		{
			if (_gameInfo.State == GameState.NotStarted)
			{
				return StartX;
			}

			// Move forward according to time
			var distance = time * _gameInfo.RaftSpeed;

			return StartX + distance;
		}
    }
}
