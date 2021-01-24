using System;
using MvvmCross.ViewModels;
using Physics.CompoundOscillations.Logic;

namespace Physics.CompoundOscillations.ViewModels
{
	public class OscillationInfoViewModel : MvxNotifyPropertyChanged
	{
		private IOscillationPhysicsService _physicsService = null;

		public OscillationInfoViewModel(OscillationInfo throwInfo)
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
			TimeElapsed = timeElapsed.ToString("0.##") + " s";
			var currentY = _physicsService.CalculateY(timeElapsed);
			CurrentY = currentY.ToString("0.##") + " m";
		}

		public string TimeElapsed { get; private set; }

		public string CurrentY { get; private set; }

		public bool IsVisible { get; set; } = true;

		public bool IsEnabled { get; set; } = true;
	}
}
