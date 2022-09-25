using Physics.OpticalInstruments.Rendering;
using Physics.OpticalInstruments.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.OpticalInstruments.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, GameCanvasController>
	{
		public GameViewBase()
		{
		}		

		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GameCanvasController CreateController(ISkiaCanvas canvas) => Model.CreateController(canvas);
	}
}
