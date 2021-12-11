using Physics.Shared.UI.Infrastructure;
using Physics.OpticalInstruments.Infrastructure;

namespace Physics.OpticalInstruments
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "33d55242-9c2f-46f7-be1e-eceeec195567";
	}
}
