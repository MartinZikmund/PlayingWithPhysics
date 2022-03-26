using MvvmCross;
using MvvmCross.IoC;
using Physics.Shared.UI.Infrastructure;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.WaveInterference.Infrastructure;

namespace Physics.WaveInterference.Core
{
	public class AppSetup : DefaultAppSetup<DefaultApp>
	{
		protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
		{
			base.InitializeFirstChance(iocProvider);
			iocProvider.LazyConstructAndRegisterSingleton<ITopicConfiguration, TopicNavigator>();
		}
	}
}
