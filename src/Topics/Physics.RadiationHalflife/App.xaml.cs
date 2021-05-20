using Physics.RadiationHalflife.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.RadiationHalflife
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "07bae9c9-c02d-4d85-84c8-a99aa0884d2c";
	}
}
