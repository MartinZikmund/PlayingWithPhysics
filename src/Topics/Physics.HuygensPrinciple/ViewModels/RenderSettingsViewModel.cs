using MvvmCross.ViewModels;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class RenderSettingsViewModel : MvxNotifyPropertyChanged
	{
		public float StepRadius { get; set; } = 12.5f;

		public int FieldSize { get; set; } = 1080;
	}
}
