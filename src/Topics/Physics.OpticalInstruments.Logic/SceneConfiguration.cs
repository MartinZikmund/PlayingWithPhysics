using MvvmCross.ViewModels;

namespace Physics.OpticalInstruments.Logic
{
	public class SceneConfiguration : MvxNotifyPropertyChanged
	{
		public float FocalDistance { get; set; } = 5;

		public string DioptersString => (1 / FocalDistance).ToString("0.##");

		public float ObjectHeight { get; set; } = 5;

		public float ObjectDistance { get; set; } = 10;
	}
}
