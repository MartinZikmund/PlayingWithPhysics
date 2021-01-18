using Physics.LissajousCurves.Rendering;
using Physics.LissajousCurves.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.LissajousCurves.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView()
		{
			this.InitializeComponent();
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, CompoundOscillationsController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new NonUiSkiaCanvas();

		protected override CompoundOscillationsController CreateController(ISkiaCanvas canvas) => new CompoundOscillationsController(canvas);
	}
}
