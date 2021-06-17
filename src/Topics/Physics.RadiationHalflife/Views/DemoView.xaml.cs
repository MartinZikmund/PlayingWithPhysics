using System;
using System.Linq;
using Physics.RadiationHalflife.Rendering;
using Physics.RadiationHalflife.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.RadiationHalflife.Views
{
	public sealed partial class DemoView : DemoViewBase
	{
		private BitmapImage _currentGif = new BitmapImage();
		public DemoView()
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

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Model.SelectedNucleoid == Model.Nucleoids.Last())
			{
				MainScrollView.ChangeView(0.0f, double.MaxValue, 1.0f);
			}
		}
	}

	public class DemoViewBase : BaseSkiaView<DemoViewModel, RadiationHalflifeController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override RadiationHalflifeController CreateController(ISkiaCanvas canvas) => new RadiationHalflifeController(canvas);
	}
}
