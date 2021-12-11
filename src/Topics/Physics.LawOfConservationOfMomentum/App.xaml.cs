using Physics.Shared.UI.Infrastructure;
using Physics.LawOfConservationOfMomentum.Infrastructure;

namespace Physics.LawOfConservationOfMomentum
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "c65582db-eec5-4938-9334-a8ca0d292e7e";
	}
}
