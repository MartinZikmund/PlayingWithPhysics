using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Physics.InclinedPlane.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.InclinedPlane
{
	sealed partial class App : PhysicsApp
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("a6b20ac1-6216-4862-9c2a-dccf5b6df820",
                   typeof(Analytics), typeof(Crashes));
        }   
    }
    public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
    {
    }
}
