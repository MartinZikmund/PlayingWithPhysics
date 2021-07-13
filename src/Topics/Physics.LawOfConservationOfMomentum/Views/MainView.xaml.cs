using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.LawOfConservationOfMomentum.Rendering;
using Physics.LawOfConservationOfMomentum.ViewModels;

namespace Physics.LawOfConservationOfMomentum.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, LawOfConservationOfMomentumCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override LawOfConservationOfMomentumCanvasController CreateController(ISkiaCanvas canvas) =>
			new LawOfConservationOfMomentumCanvasController(canvas);
	}
}
