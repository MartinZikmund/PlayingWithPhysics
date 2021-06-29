using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.RotationalInclinedPlane.Rendering;
using Physics.RotationalInclinedPlane.ViewModels;

namespace Physics.RotationalInclinedPlane.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, RotationalInclinedPlaneCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override RotationalInclinedPlaneCanvasController CreateController(ISkiaCanvas canvas)
		{
			var controller = new RotationalInclinedPlaneCanvasController(canvas);
			controller.SetVariantRenderer(new SkiaSimulationRenderer(controller));
			return controller;
		}
	}
}
