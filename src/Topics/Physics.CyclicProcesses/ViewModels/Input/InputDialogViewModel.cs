using MvvmCross.ViewModels;

namespace Physics.CyclicProcesses.ViewModels.Input
{
	public class InputDialogViewModel : MvxNotifyPropertyChanged
	{
		public float V { get; set; }

		public float V1 { get; set; }

		public float V2 { get; set; }

		public float T1 { get; set; }

		public float T2 { get; set; }

		public float T12 { get; set; }

		public float T34 { get; set; }

		public float P { get; set; }

		public float P1 { get; set; }
	}
}
