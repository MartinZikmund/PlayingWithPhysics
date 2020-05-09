using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Uap.Core;
using Physics.HomogenousParticle.Core;
using Physics.HomogenousParticle.Infrastructure.Topics;
using Physics.Shared;
using Physics.Shared.ViewModels;
using System.Collections.Generic;
using System.Reflection;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HomogenousParticle.Core
{
    public class CrossSetup : DefaultAppSetup<CrossApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITopicConfiguration, TopicNavigator>();
        }
    }
}
