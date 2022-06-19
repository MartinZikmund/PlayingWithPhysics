using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.GravitationalFieldMovement.Rendering;
using Physics.GravitationalFieldMovement.ViewModels;
using Physics.Shared.UI.Helpers;

namespace Physics.GravitationalFieldMovement.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView()
		{
			InitializeComponent();
			SetupNumberBoxFormatting();
		}
		public void SetupNumberBoxFormatting()
		{
			MaxTInput.SetupFormatting(0.001, 1, 3, 0.001, 0.001);
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, GravitationalFieldMovementCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GravitationalFieldMovementCanvasController CreateController(ISkiaCanvas canvas) =>
			new GravitationalFieldMovementCanvasController(canvas);
	}
}
