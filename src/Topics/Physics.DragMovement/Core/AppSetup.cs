using MvvmCross;
using MvvmCross.IoC;
using Physics.DragMovement.Infrastructure.Topics;
using Physics.Shared.UI.Infrastructure;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.DragMovement.Core
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
