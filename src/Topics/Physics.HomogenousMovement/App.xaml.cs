using Physics.HomogenousMovement.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.HomogenousMovement
{
	sealed partial class App
    {
		public App() => InitializeComponent();
	}

    public class HomogenousMovementApp : PhysicsAppBase<AppSetup, DefaultApp<AppStart>>
    {
		protected override string AppCenterKey => "1d9d6ae2-9f90-4992-9ec2-a9811882f3dc";
	}
}
