using Physics.Shared.UI.Infrastructure;
using Physics.RotationalInclinedPlane.Infrastructure;

namespace Physics.RotationalInclinedPlane
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "884548a2-3a49-4111-ab8a-bab159374fe3";
	}
}
