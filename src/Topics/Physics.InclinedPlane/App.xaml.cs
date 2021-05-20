using Physics.InclinedPlane.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.InclinedPlane
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}
	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "a6b20ac1-6216-4862-9c2a-dccf5b6df820";
	}
}
