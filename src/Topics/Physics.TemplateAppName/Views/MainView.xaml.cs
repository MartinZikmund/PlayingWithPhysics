using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.TemplateAppName.Rendering;
using Physics.TemplateAppName.ViewModels;

namespace Physics.TemplateAppName.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, TemplateAppNameCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override TemplateAppNameCanvasController CreateController(ISkiaCanvas canvas) =>
			new TemplateAppNameCanvasController(canvas);
	}
}
