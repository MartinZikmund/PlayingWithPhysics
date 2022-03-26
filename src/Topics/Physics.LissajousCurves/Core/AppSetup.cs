using MvvmCross;
using MvvmCross.IoC;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Infrastructure;
using Physics.LissajousCurves.Infrastructure.Topics;

namespace Physics.LissajousCurves.Core
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
