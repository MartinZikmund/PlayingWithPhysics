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
		protected override string AppCenterKey => "TemplateAppCenterKey";
	}
}
