using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.FluidFlow.Rendering;
using Physics.FluidFlow.ViewModels;

namespace Physics.FluidFlow.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, FluidFlowCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override FluidFlowCanvasController CreateController(ISkiaCanvas canvas) =>
			new FluidFlowCanvasController(canvas);
	}
}
