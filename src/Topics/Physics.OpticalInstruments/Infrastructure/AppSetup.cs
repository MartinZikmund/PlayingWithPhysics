using MvvmCross;
using MvvmCross.IoC;
using Physics.Shared.UI.Infrastructure;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.OpticalInstruments.Infrastructure
{
	public class AppSetup : DefaultAppSetup<DefaultApp>
	{
		protected override void InitializeFirstChance()
		{
			base.InitializeFirstChance();
			Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITopicConfiguration, TopicNavigator>();
		}
	}
}
