using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace Physics.OpticalInstruments.Game
{
	public class GameInfo : MvxNotifyPropertyChanged
	{
		internal int ObjectX => 1300;

		private Random _randomizer = new Random();

		public int TargetAngle { get; private set; }

		public event EventHandler GameStateChanged;

		public void OnStateChanged() => GameStateChanged?.Invoke(this, EventArgs.Empty);

		public GameState State { get; private set; }

		public int CurrentShot { get; private set; } = 1;

		public int TotalShots => 3;

		public int CurrentAngle { get; set; } = 20;

		public double? BestScore { get; private set; }

		public int FinishedAttempts { get; private set; }

		public List<double> Attempts { get; } = new List<double>();

		public string Attempt1Text => Attempts.Count > 0 ? Attempts[0].ToString() : "";

		public string Attempt2Text => Attempts.Count > 1 ? Attempts[1].ToString() : "";

		public string Attempt3Text => Attempts.Count > 2 ? Attempts[2].ToString() : "";

		public void Shoot()
		{
			FinishedAttempts++;
			if (FinishedAttempts == 3)
			{
				State = GameState.GameEnded;
			}
			else
			{
				State = GameState.Fired;
			}
			double score = CalculateScore();
			Attempts.Add(score);
			BestScore = BestScore is null ? score : Math.Max(score, BestScore.Value);
			RaiseAllPropertiesChanged();
		}

		public void StartNewGame()
		{
			GenerateRandomTarget();
			State = GameState.SetAngle;
			CurrentShot = 1;
			Attempts.Clear();
			BestScore = null;
			FinishedAttempts = 0;
			RaiseAllPropertiesChanged();
		}


		public void NextShot()
		{
			State = GameState.SetAngle;
			CurrentShot++;
			RaiseAllPropertiesChanged();
		}

		private void GenerateRandomTarget()
		{
			TargetAngle = _randomizer.Next(37, 63);
		}

		internal void MockAngle()
		{
			TargetAngle = CurrentAngle;
		}

		private double CalculateScore()
		{
			var diff = Math.Min(Math.Abs(CurrentAngle - TargetAngle), 20);
			var score = 1000 - diff * 50;
			return score;
		}
	}
}
