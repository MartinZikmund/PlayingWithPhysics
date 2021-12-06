using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.CyclicProcesses.Rendering;
using Physics.CyclicProcesses.ViewModels;
using System;

namespace Physics.CyclicProcesses.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();

		private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			switch(AnimationSelection.SelectedIndex)
			{
				case 0:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/adiabat.html"));
					break;
				case 1:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/izobar.html"));
					break;
				case 2:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/izoterm.html"));
					break;
				case 3:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/izochor.html"));
					break;
				case 4:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/motor.html"));
					break;
			}
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, CyclicProcessesCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override CyclicProcessesCanvasController CreateController(ISkiaCanvas canvas) =>
			new CyclicProcessesCanvasController(canvas);
	}
}
