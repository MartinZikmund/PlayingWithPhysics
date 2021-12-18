using MvvmCross.ViewModels;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class RenderConfigurationViewModel : MvxViewModel
    {
		public bool ShowWaveEdge { get; set; } = false;

		public bool ShowWave { get; set; } = true;

		public bool ShowSignificantPoints { get; set; } = false;

		public bool ShowObject { get; set; } = true;
	}
}
