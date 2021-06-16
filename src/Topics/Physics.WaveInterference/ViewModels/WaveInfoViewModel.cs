﻿using System;
using MvvmCross.ViewModels;
using Physics.WaveInterference.Logic;

namespace Physics.WaveInterference.ViewModels
{
	public class WaveInfoViewModel : MvxNotifyPropertyChanged
	{
		private IOscillationPhysicsService _physicsService = null;

		public WaveInfoViewModel(WaveInfo throwInfo)
		{
			WaveInfo = throwInfo ?? throw new ArgumentNullException(nameof(throwInfo));
		}

		private WaveInfo _waveInfo;

		public WaveInfo WaveInfo
		{
			get
			{
				return _waveInfo;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_physicsService = new OscillationPhysicsService(value);
				SetProperty(ref _waveInfo, value);
			}
		}

		public string Label
		{
			get => WaveInfo.Label;
			set
			{
				WaveInfo.Label = value;
				RaisePropertyChanged();
			}
		}

		public void UpdateCurrentValues(float timeElapsed)
		{
			TimeElapsed = timeElapsed.ToString("0.##");
			var currentY = _physicsService.CalculateY(timeElapsed);
			CurrentYText = currentY.ToString(" 0.00;-0.00; 0.00");
			RaisePropertyChanged(nameof(WaveInfo));
			RaisePropertyChanged(nameof(PeriodText));
		}


		public string WaveLengthText => WaveInfo.WaveLengthText;
		//public string PhaseText => WaveInfo.PhaseInRad;
		public string PeriodText => (1.0f / WaveInfo.Frequency).ToString("0.000");

		public string TimeElapsed { get; private set; }

		public string CurrentYText { get; private set; }

		public bool IsVisible { get; set; } = true;

		public bool IsEnabled { get; set; } = true;
	}
}
