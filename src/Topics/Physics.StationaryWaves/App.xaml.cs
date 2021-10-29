using Physics.Shared.UI.Infrastructure;
using Physics.StationaryWaves.Infrastructure;

namespace Physics.StationaryWaves
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "TemplateAppCenterKey";
	}
}
