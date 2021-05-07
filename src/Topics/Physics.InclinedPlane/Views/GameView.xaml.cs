using Physics.InclinedPlane.Rendering;
using Physics.InclinedPlane.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.InclinedPlane.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			this.InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, InclinedPlaneSkiaController>
	{
		private SkiaCanvas _animatedCanvas;

		protected override ISkiaCanvas CreateSkiaCanvas()
		{
			_animatedCanvas = new SkiaCanvas();
			_animatedCanvas.PointerMoved += _animatedCanvas_PointerMoved;
			_animatedCanvas.PointerExited += _animatedCanvas_PointerExited;
			_animatedCanvas.PointerPressed += _animatedCanvas_PointerPressed;
			return _animatedCanvas;
		}

		private void _animatedCanvas_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			Model.PreviewStone(null);
		}

		private void _animatedCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(_animatedCanvas);
			Model.CanvasTapped((float)point.Position.X);
		}

		private void _animatedCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(_animatedCanvas);
			Model.PreviewStone((float)point.Position.X);
		}

		protected override InclinedPlaneSkiaController CreateController(ISkiaCanvas canvas)
		{
			var canvasController = new InclinedPlaneSkiaController(_animatedCanvas);
			canvasController.SetVariantRenderer(new GameRenderer(canvasController));

			return canvasController;
		}
	}
}
