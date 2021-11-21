using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.HuygensPrinciple.Rendering;
using Physics.HuygensPrinciple.ViewModels;
using System.Linq;
using System;
using Windows.UI.Xaml;

namespace Physics.HuygensPrinciple.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView() => InitializeComponent();

		private void CanvasHolder_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
			var child = CanvasHolder.Children.FirstOrDefault() as FrameworkElement;
			if (child == null)
			{
				return;
			}

			// Resize to square
			var squareSize = Math.Min(e.NewSize.Width, e.NewSize.Height) - 0;
			child.Width = squareSize;
			child.Height = squareSize;
			child.HorizontalAlignment = HorizontalAlignment.Center;
			child.VerticalAlignment = VerticalAlignment.Center;
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, HuygensPrincipleCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override HuygensPrincipleCanvasController CreateController(ISkiaCanvas canvas) =>
			new HuygensPrincipleCanvasController(canvas);
	}
}
