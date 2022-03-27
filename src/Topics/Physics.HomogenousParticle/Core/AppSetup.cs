using MvvmCross;
using MvvmCross.IoC;
using Physics.HomogenousParticle.Infrastructure.Topics;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Infrastructure;

namespace Physics.HomogenousParticle.Core
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
