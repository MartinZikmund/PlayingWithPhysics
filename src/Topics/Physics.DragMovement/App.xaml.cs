using Physics.DragMovement.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.DragMovement
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "0e9f31ca-d429-4990-9a07-5e1358264e78";
	}
}
