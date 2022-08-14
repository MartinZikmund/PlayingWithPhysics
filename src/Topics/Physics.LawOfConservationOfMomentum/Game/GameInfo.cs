using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.ViewModels;
using Physics.LawOfConservationOfMomentum.Logic;

namespace Physics.LawOfConservationOfMomentum.Game
{
	public class GameInfo : MvxNotifyPropertyChanged
	{
		private readonly Random _randomizer = new Random();

		public event EventHandler GameStateChanged;

		public void OnStateChanged() => GameStateChanged?.Invoke(this, EventArgs.Empty);

		public GamePhysicsService PhysicsService { get; private set; }

		public GameAttemptPhysicsService AttemptPhysicsService { get; private set; }

		public GameState State { get; private set; }

		public float BallMass { get; private set; }

		public float BaronMass { get; private set; }

		public float NetPeriod { get; private set; }

		public float InitialVelocity { get; private set; }

		public float? CurrentFireTime { get; private set; }

		public int CurrentShot { get; private set; } = 1;

		public int TotalAttempts => 3;

		public float? BestDistance => Attempts.Count > 0 ? Attempts.Min() : null;

		public string BestDistanceText => BestDistance is not null ? BestDistance.Value.ToString("0.0") : "";

		public int FinishedAttempts { get; private set; }

		public string Attempt1DistanceText => Attempts.Count > 0 ? Attempts[0].ToString("0.0") : "";

		public string Attempt2DistanceText => Attempts.Count > 1 ? Attempts[1].ToString("0.0") : "";

		public string Attempt3DistanceText => Attempts.Count > 2 ? Attempts[2].ToString("0.0") : "";

		public List<float> Attempts { get; } = new List<float>();

		public void StartNewGame()
		{
			State = GameState.WaitingForFiring;
			BallMass = _randomizer.Next(1, 5);
			BaronMass = _randomizer.Next(1, 5);
			NetPeriod = _randomizer.Next(10, 21) / 10.0f;
			CurrentFireTime = null;
			CurrentShot = 1;
			Attempts.Clear();
			FinishedAttempts = 0;
			RaiseAllPropertiesChanged();
			PhysicsService = new GamePhysicsService(BallMass, BaronMass, NetPeriod);
			AttemptPhysicsService = null;
		}

		public void NextShot()
		{
			State = GameState.WaitingForFiring;
			CurrentFireTime = null;
			CurrentShot++;
			AttemptPhysicsService = null;
		}

		internal void Fire(float currentSimulationTime)
		{
			CurrentFireTime = currentSimulationTime;
			AttemptPhysicsService = PhysicsService.StartAttempt(InitialVelocity, currentSimulationTime);
			State = GameState.Fired;
		}

		internal void SimulationEnded()
		{
			var endTime = AttemptPhysicsService.CalculateEndTime() + CurrentFireTime.Value;
			var endDistance = AttemptPhysicsService.CurrentDistance(endTime);
			FinishedAttempts++;
			Attempts.Add(endDistance);
			if (FinishedAttempts == 3)
			{
				State = GameState.GameEnded;
			}
			else
			{
				State = GameState.SimulationEnded;
			}
			RaiseAllPropertiesChanged();
		}
	}
}
