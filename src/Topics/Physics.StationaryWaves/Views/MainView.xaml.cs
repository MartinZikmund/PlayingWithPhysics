using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.StationaryWaves.Rendering;
using Physics.StationaryWaves.ViewModels;

namespace Physics.StationaryWaves.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();

		internal void FocusSimulationControls() => SimulationControls.Focus(Windows.UI.Xaml.FocusState.Programmatic);
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, StationaryWavesCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override StationaryWavesCanvasController CreateController(ISkiaCanvas canvas) =>
			new StationaryWavesCanvasController(canvas);
	}
}
