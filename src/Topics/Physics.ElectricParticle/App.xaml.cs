using Physics.ElectricParticle.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.ElectricParticle
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "1e690bca-3a2e-46ff-ad7f-affbd31466ba";
	}
}
