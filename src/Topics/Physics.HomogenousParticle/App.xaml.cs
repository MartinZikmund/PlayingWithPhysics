using Physics.HomogenousParticle.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.HomogenousParticle
{
	sealed partial class App : PhysicsApp
    {
		public App() => InitializeComponent();
	}

    public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
    {
		protected override string AppCenterKey => "d6d547de-d761-45a8-8c8f-fd508f540d1e";
	}
}
