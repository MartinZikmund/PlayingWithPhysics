using Physics.Shared.UI.Infrastructure;
using Physics.HuygensPrinciple.Infrastructure;

namespace Physics.HuygensPrinciple
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
