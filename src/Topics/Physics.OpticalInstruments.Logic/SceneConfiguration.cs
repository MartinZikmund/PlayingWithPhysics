using MvvmCross.ViewModels;

namespace Physics.OpticalInstruments.Logic
{
	public class SceneConfiguration : MvxNotifyPropertyChanged
	{
		public float FocalDistance { get; set; } = 5;

		public float ObjectHeight { get; set; }

		public float ObjectDistance { get; set; }
	}
}
