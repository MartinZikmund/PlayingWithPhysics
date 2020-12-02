using Physics.CompoundOscillations.Rendering;
using Physics.CompoundOscillations.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.CompoundOscillations.Views
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
		protected override CompoundOscillationsController CreateController(SkiaCanvas canvas) => throw new System.NotImplementedException();
	}
}
