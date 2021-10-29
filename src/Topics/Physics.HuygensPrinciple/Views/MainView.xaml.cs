using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.HuygensPrinciple.Rendering;
using Physics.HuygensPrinciple.ViewModels;

namespace Physics.HuygensPrinciple.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, HuygensPrincipleCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override HuygensPrincipleCanvasController CreateController(ISkiaCanvas canvas) =>
			new HuygensPrincipleCanvasController(canvas);
	}
}
