using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.CyclicProcesses.Rendering;
using Physics.CyclicProcesses.ViewModels;

namespace Physics.CyclicProcesses.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, CyclicProcessesCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override CyclicProcessesCanvasController CreateController(ISkiaCanvas canvas) =>
			new CyclicProcessesCanvasController(canvas);
	}
}
