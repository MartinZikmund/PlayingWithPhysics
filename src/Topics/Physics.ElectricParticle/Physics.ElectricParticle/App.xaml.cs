using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Physics.ElectricParticle.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.ElectricParticle
{
	sealed partial class App : PhysicsApp
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("d6d547de-d761-45a8-8c8f-fd508f540d1e",
                   typeof(Analytics), typeof(Crashes));
        }
    }

    public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
    {
    }
}
