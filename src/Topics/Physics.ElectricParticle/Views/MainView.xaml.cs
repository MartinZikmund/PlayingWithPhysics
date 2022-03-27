using Physics.ElectricParticle.Rendering;
using Physics.ElectricParticle.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.ElectricParticle.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView()
		{
			this.InitializeComponent();
		}

		internal void FocusSimulationControls()
		{
			SimulationControls.Focus(Windows.UI.Xaml.FocusState.Programmatic);
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, ElectricParticleCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override ElectricParticleCanvasController CreateController(ISkiaCanvas canvas) => new ElectricParticleCanvasController(canvas);
	}
}
