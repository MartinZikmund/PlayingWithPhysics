using Physics.LissajousCurves.Core;
using Physics.Shared.UI.Infrastructure;

namespace Physics.LissajousCurves
{
	sealed partial class App : PhysicsApp
	{
		public App()
		{
			this.InitializeComponent();
		}

	}
	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
		protected override string AppCenterKey => "e05d8c56-7780-4140-9933-b8dec7d20208";
	}
}
