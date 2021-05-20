using Physics.CompoundOscillations.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.CompoundOscillations
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}

	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "7648efa6-50ff-4a96-b01b-94e0d8b11dca";
	}
}
