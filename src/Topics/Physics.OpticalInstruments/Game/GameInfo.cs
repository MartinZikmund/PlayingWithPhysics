using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace Physics.OpticalInstruments.Game
{
	public class GameInfo : MvxNotifyPropertyChanged
    {
        public GameState State { get; private set; }

        public int CurrentShot { get; private set; } = 1;        

        public int TotalThrows => 3;

		public int CurrentAngle { get; set; } = 20;
		
		public float? TotalDistance { get; private set; }

        public int FinishedAttempts { get; private set; }

		public List<float> Attempts { get; }
		
        public void AddAttempt(float distance)
        {
            FinishedAttempts++;
			Attempts.Add(distance);
            TotalDistance = (TotalDistance ?? 0) + distance;
        }

        public float AverageDistance => TotalDistance != null ? TotalDistance.Value / FinishedAttempts : float.NaN;

		public void StartNewGame()
		{
			State = GameState.SetAngle;
			CurrentShot = 1;
			Attempts.Clear();
			TotalDistance = null;
			FinishedAttempts = 0;
		}

		public void NextShot()
		{
			State = GameState.SetAngle;
			CurrentShot++;
		}
    }
}
