using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.GravitationalFieldMovement.Rendering;
using Physics.GravitationalFieldMovement.ViewModels;

namespace Physics.GravitationalFieldMovement.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, GravitationalFieldMovementCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GravitationalFieldMovementCanvasController CreateController(ISkiaCanvas canvas) =>
			new GravitationalFieldMovementCanvasController(canvas);
	}
}
