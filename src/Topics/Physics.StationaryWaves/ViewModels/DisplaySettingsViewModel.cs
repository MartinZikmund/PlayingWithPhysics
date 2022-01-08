using MvvmCross.ViewModels;

namespace Physics.StationaryWaves.ViewModels
{
	public class DisplaySettingsViewModel : MvxNotifyPropertyChanged
	{
		public bool ShowWavePackage { get; set; } = true;

		public bool ShowBaseWaves { get; set; } = true;

		public bool ShowResultingWave { get; set; } = true;
	}
}
