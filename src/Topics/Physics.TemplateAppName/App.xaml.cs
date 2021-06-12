using Physics.Shared.UI.Infrastructure;
using Physics.TemplateAppName.Infrastructure;

namespace Physics.TemplateAppName
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
