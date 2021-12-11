using Physics.Shared.UI.Infrastructure;
using Physics.FluidFlow.Infrastructure;

namespace Physics.FluidFlow
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "374dcb15-a616-4309-aede-904789443db5";
	}
}
