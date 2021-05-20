using Physics.Shared.UI.Infrastructure;
using Physics.WaveInterference.Core;

namespace Physics.WaveInterference
{
	sealed partial class App : PhysicsApp
	{
		public App() => InitializeComponent();
	}
	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "18f4479a-8691-4208-926e-0bae37e978ec";
	}
}
