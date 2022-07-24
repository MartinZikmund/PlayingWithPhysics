using Physics.LawOfConservationOfMomentum.Rendering;
using Physics.LawOfConservationOfMomentum.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.LawOfConservationOfMomentum.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, GameController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GameController CreateController(ISkiaCanvas canvas)
		{
			var controller = new GameController(canvas);
			return controller;
		}
	}
}
