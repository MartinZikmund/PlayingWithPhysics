using System;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Localization;
using Physics.StationaryWaves.Logic;

namespace Physics.StationaryWaves.ViewModels
{
	public class WaveInfoViewModel : MvxNotifyPropertyChanged
	{
		private EasyWavePhysicsService _physicsService = null;

		public WaveInfoViewModel(WaveInfo throwInfo)
		{
			WaveInfo = throwInfo ?? throw new ArgumentNullException(nameof(throwInfo));
		}

		private WaveInfo _waveInfo;

		public WaveInfo WaveInfo
		{
			get => _waveInfo;
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				//_physicsService = new WavePhysicsService(value);
				SetProperty(ref _waveInfo, value);
			}
		}

		public EasyWavePhysicsService PhysicsService => _physicsService;

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
			//var currentY = _physicsService.CalculateY(0, timeElapsed);
			//var currentY = 0f;
			//CurrentYText = currentY?.ToString(" 0.00;-0.00; 0.00") ?? Constants.NoValueString;
			RaisePropertyChanged(nameof(WaveInfo));
		}

		public string AVariantText
		{
			get
			{
				switch (WaveInfo.A)
				{
					case AVariant.Pi:
						return Localizer.Instance["AVariantPiLabel"];
					case AVariant.Zero:
					default:
						return Localizer.Instance["AVariantZeroLabel"];
				}
			}
		}
		public string TimeElapsed { get; private set; }

		public string CurrentYText { get; private set; }

		public bool IsVisible { get; set; } = true;

		public bool IsEnabled { get; set; } = true;
	}
}
