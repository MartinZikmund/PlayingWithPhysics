using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.OpticalInstruments.Rendering;
using Physics.OpticalInstruments.ViewModels;

namespace Physics.OpticalInstruments.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView()
		{
			InitializeComponent();
			InkCanvas.PointerMoved += InkCanvasPointerManipulation;
			InkCanvas.PointerPressed += InkCanvasPointerManipulation;
			UpdateInkPresenterInput();
		}

		private void InkCanvasPointerManipulation(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (InkToolbarSwitch.IsOn || !e.Pointer.IsInContact)
			{
				return;
			}

			var desiredPoint = e.GetCurrentPoint(InkCanvas);
			Model.TrySetObject(desiredPoint.Position);
		}

		private void InkToolbarSwitch_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e) =>
			UpdateInkPresenterInput();

		private void UpdateInkPresenterInput() =>
			InkCanvas.InkPresenter.IsInputEnabled = InkToolbarSwitch.IsOn;
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, OpticalInstrumentsCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override OpticalInstrumentsCanvasController CreateController(ISkiaCanvas canvas) =>
			new OpticalInstrumentsCanvasController(canvas);
	}
}
