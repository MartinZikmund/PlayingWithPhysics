using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.OpticalInstruments.Rendering;
using Physics.OpticalInstruments.ViewModels;

namespace Physics.OpticalInstruments.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, OpticalInstrumentsCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override OpticalInstrumentsCanvasController CreateController(ISkiaCanvas canvas) =>
			new OpticalInstrumentsCanvasController(canvas);
	}
}
