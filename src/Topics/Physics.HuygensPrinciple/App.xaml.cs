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
		protected override string AppCenterKey => "f10ed737-cc13-4f69-bfa8-affa0e35925c";
	}
}
