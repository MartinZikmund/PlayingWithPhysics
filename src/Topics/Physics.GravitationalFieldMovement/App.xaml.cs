using Physics.Shared.UI.Infrastructure;
using Physics.GravitationalFieldMovement.Infrastructure;

namespace Physics.GravitationalFieldMovement
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "c635e0ac-21d7-462b-af83-dd08fbc4a51c";
	}
}
