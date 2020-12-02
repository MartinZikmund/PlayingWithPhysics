using Physics.InclinedPlane.Rendering;
using Physics.InclinedPlane.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.InclinedPlane.Views
{
	public sealed partial class MainView : MainViewBase
    {
        public MainView()
        {
            this.InitializeComponent();
        }
    }

	public abstract class MainViewBase : BaseSkiaView<MainViewModel, InclinedPlaneSkiaController>
	{
		protected override InclinedPlaneSkiaController CreateController(SkiaCanvas canvas)
		{
			var controller = new InclinedPlaneSkiaController(canvas);
			controller.SetVariantRenderer(new SkiaSimulationRenderer(controller));
			return controller;
		}
	}
}
