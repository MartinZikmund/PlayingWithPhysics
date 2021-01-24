using Physics.CompoundOscillations.Rendering;
using Physics.CompoundOscillations.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.CompoundOscillations.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			this.InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, AngryDirectorController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override AngryDirectorController CreateController(ISkiaCanvas canvas) => Model.CreateController(canvas);
	}
}
