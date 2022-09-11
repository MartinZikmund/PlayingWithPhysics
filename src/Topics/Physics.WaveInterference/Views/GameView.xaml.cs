using Physics.WaveInterference.Rendering;
using Physics.WaveInterference.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.WaveInterference.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			this.InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, GameController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GameController CreateController(ISkiaCanvas canvas) =>
			new GameController(canvas);
	}
}
