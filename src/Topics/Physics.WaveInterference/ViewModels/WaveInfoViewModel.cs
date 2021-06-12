using System;
using MvvmCross.ViewModels;
using Physics.WaveInterference.Logic;

namespace Physics.WaveInterference.ViewModels
{
	public class WaveInfoViewModel : MvxNotifyPropertyChanged
	{
		private IOscillationPhysicsService _physicsService = null;

		public WaveInfoViewModel(OscillationInfo throwInfo)
		{
			OscillationInfo = throwInfo ?? throw new ArgumentNullException(nameof(throwInfo));
		}

		private OscillationInfo _oscillationInfo;

		public OscillationInfo OscillationInfo
		{
			get
			{
				return _oscillationInfo;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_physicsService = new OscillationPhysicsService(value);
				SetProperty(ref _oscillationInfo, value);
			}
		}

		public string Label
		{
			get => OscillationInfo.Label;
			set
			{
				OscillationInfo.Label = value;
				RaisePropertyChanged();
			}
		}

		public void UpdateCurrentValues(float timeElapsed)
		{
			TimeElapsed = timeElapsed.ToString("0.##");
			var currentY = _physicsService.CalculateY(timeElapsed);
			CurrentYText = currentY.ToString(" 0.00;-0.00; 0.00");
			RaisePropertyChanged(nameof(OscillationInfo));
			RaisePropertyChanged(nameof(PeriodText));
		}

		public string PeriodText => (1 / OscillationInfo.Frequency).ToString("0.000");

		public string TimeElapsed { get; private set; }

		public string CurrentYText { get; private set; }

		public bool IsVisible { get; set; } = true;

		public bool IsEnabled { get; set; } = true;
	}
}
