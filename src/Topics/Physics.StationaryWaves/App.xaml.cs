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
		protected override string AppCenterKey => "510fc6d3-9c2d-4489-a859-0cf09ad31d78";
	}
}
