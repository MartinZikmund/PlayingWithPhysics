using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Physics.CompoundOscillations.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.CompoundOscillations
{
	sealed partial class App : PhysicsApp
	{
		public App()
		{
			this.InitializeComponent();
			AppCenter.Start("7648efa6-50ff-4a96-b01b-94e0d8b11dca",
				   typeof(Analytics), typeof(Crashes));
		}
	}
	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
	}
}
