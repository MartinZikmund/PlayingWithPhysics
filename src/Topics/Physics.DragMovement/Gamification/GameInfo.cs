﻿using System;
using MvvmCross.ViewModels;

namespace Physics.DragMovement.Gamification
{
	public class GameInfo : MvxNotifyPropertyChanged
	{
		private int _cargoMass = 15;

		public GameInfo(double raftSpeed, double helicopterHeight)
		{
			RaftSpeed = raftSpeed;
			HelicopterAltitude = helicopterHeight;
		}

		public GameState State { get; private set; } = GameState.NotStarted;

		public double RaftSpeed { get; }

		public string FormattedRaftSpeed => RaftSpeed.ToString("0.0");

		public double HelicopterAltitude { get; }

		public bool IsStartVisible => State == GameState.NotStarted;

		public bool IsDropVisible => State == GameState.Started || State == GameState.Dropped;

		public bool IsDropEnabled => State != GameState.Dropped;

		public bool IsRestartEnabled => State != GameState.NotStarted;

		public bool IsWeightChangeEnabled => State <= GameState.Started;

		public TimeSpan DropTime { get; set; }

		public TimeSpan HitTime { get; set; }

		public bool WillDropOnRaft { get; set; }

		public bool AreSoundsEnabled { get; set; }

		public int CargoMass
		{
			get => _cargoMass;
			set
			{
				value = Math.Clamp(value, 15, 45);
				SetProperty(ref _cargoMass, value);
			}
		}

		public void SetState(GameState state)
		{
			State = state;
			RaisePropertyChanged(nameof(IsStartVisible));
			RaisePropertyChanged(nameof(IsDropVisible));
			RaisePropertyChanged(nameof(IsDropEnabled));
			RaisePropertyChanged(nameof(IsRestartEnabled));
			RaisePropertyChanged(nameof(IsWeightChangeEnabled));
		}
	}
}
