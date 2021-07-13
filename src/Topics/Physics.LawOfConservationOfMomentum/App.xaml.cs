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
		protected override string AppCenterKey => "TemplateAppCenterKey";
	}
}
