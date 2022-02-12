using MvvmCross.ViewModels;

namespace Physics.HuygensPrinciple.ViewModels
{
	public class RenderConfigurationViewModel : MvxViewModel
    {
		public bool ShowWaveEdge { get; set; } = true;

		public bool ShowWave { get; set; } = false;

		public bool ShowSignificantPoints { get; set; } = true;

		public bool ShowObject { get; set; } = true;
	}
}
