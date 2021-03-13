using System;
using Physics.RadiationHalflife.Rendering;
using Physics.RadiationHalflife.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Physics.RadiationHalflife.Views
{
	public sealed partial class MainView : MainViewBase
	{
		private BitmapImage _currentGif = new BitmapImage();
		public MainView()
		{
			this.InitializeComponent();
			_currentGif = new BitmapImage(new Uri("ms-appx:///Assets/Animations/0.gif"));
			GifCanvas.Source = _currentGif;
		}

		private void VariantsGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_currentGif = new BitmapImage(new Uri("ms-appx:///Assets/Animations/" + VariantsGroup.SelectedIndex + ".gif"));
			GifCanvas.Source = _currentGif;
		}

		private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			_currentGif.Stop();
			_currentGif.Play();
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, RadiationHalflifeController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override RadiationHalflifeController CreateController(ISkiaCanvas canvas) => new RadiationHalflifeController(canvas);
	}
}
