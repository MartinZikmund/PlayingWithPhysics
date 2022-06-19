using Physics.RotationalInclinedPlane.Rendering;
using Physics.RotationalInclinedPlane.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.RotationalInclinedPlane.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, RotationalInclinedPlaneCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override RotationalInclinedPlaneCanvasController CreateController(ISkiaCanvas canvas)
		{
			var controller = new RotationalInclinedPlaneCanvasController(canvas);
			controller.SetVariantRenderer(new GameRenderer(controller));
			return controller;
		}
	}
}
