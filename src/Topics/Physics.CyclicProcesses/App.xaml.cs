using Physics.Shared.UI.Infrastructure;
using Physics.CyclicProcesses.Infrastructure;

namespace Physics.CyclicProcesses
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "58d33c69-6560-4d25-a13d-d4e8972d691b";
	}
}
