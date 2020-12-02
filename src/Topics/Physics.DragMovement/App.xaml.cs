using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Physics.DragMovement.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.DragMovement
{
	sealed partial class App : PhysicsApp
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("0e9f31ca-d429-4990-9a07-5e1358264e78",
                   typeof(Analytics), typeof(Crashes));
        }
    }

    public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
    {
    }
}
