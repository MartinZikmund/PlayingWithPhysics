﻿using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.ViewModels;
using Physics.RotationalInclinedPlane.Logic;

namespace Physics.RotationalInclinedPlane.Game
{
	public class GameInfo : MvxNotifyPropertyChanged
	{
		private readonly Random _randomizer = new Random();
		private PhysicsService _physicsService;

		public event EventHandler GameStateChanged;

		public void OnStateChanged() => GameStateChanged?.Invoke(this, EventArgs.Empty);

		public GameState State { get; private set; }

		public int DesiredAngle { get; internal set; }

		public float DesiredTime { get; internal set; }

		public string DesiredTimeText => DesiredTime.ToString("0.0");

		public int CurrentShot { get; private set; } = 1;

		public int CurrentAngle { get; set; } = 20;

		public int TotalThrows => 3;

		public float? TotalDistance { get; private set; }

		public float? BestTime =>
			Attempts
				.Select(a => new { Attempt = a, Diff = Math.Abs(a - DesiredTime) })
				.OrderBy(a => a.Diff)
				.FirstOrDefault()?
				.Attempt;

		public string BestTimeText => BestTime is not null ? BestTime.Value.ToString("0.0") : "";

		public int FinishedAttempts { get; private set; }

		public string Attempt1TimeText => Attempts.Count > 0 ? Attempts[0].ToString("0.0") : "";

		public string Attempt2TimeText => Attempts.Count > 1 ? Attempts[1].ToString("0.0") : "";

		public string Attempt3TimeText => Attempts.Count > 2 ? Attempts[2].ToString("0.0") : "";

		public List<float> Attempts { get; } = new List<float>();

		public void AddAttempt(float actualTime)
		{
			FinishedAttempts++;
			Attempts.Add(actualTime);
			if (FinishedAttempts == 3 || actualTime == DesiredTime)
			{
				State = GameState.GameEnded;
			}
			else
			{
				State = GameState.SimulationEnded;
			}
			RaiseAllPropertiesChanged();
		}

		public float AverageDistance => TotalDistance != null ? TotalDistance.Value / FinishedAttempts : float.NaN;

		public void StartNewGame()
		{
			DesiredAngle = _randomizer.Next(5, 46);
			_physicsService = new PhysicsService(CreateGameMotionSetupWithAngle(DesiredAngle));
			DesiredTime = _physicsService.CalculateMaxT();
			State = GameState.SetAngle;
			CurrentAngle = 20;
			CurrentShot = 1;
			Attempts.Clear();
			TotalDistance = null;
			FinishedAttempts = 0;
			RaiseAllPropertiesChanged();
		}

		public void NextShot()
		{
			State = GameState.SetAngle;
			CurrentShot++;
		}

		public MotionSetup CreateGameMotionSetupWithAngle(int angle) => new MotionSetup(
				"Game object",
				BodyType.HollowCylinder,
				1,
				9.81f,
				0.5f,
				12,
				angle,
				0,
				"#000000");

		internal void StartAttempt() => State = GameState.Simulation;
	}
}
