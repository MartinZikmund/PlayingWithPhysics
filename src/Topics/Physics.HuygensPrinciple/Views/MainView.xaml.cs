using System;
using System.Linq;
using Physics.HuygensPrinciple.Logic;
using Physics.HuygensPrinciple.Rendering;
using Physics.HuygensPrinciple.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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

			DrawingSurface.Width = squareSize;
			DrawingSurface.Height = squareSize;
			DrawingSurface.HorizontalAlignment = HorizontalAlignment.Center;
			DrawingSurface.VerticalAlignment = VerticalAlignment.Center;
		}

		private void CanvasHolder_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			DrawingSurface.Children.Clear();
		}

		private void CanvasHolder_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (!Model.DrawingState.IsDrawing)
			{
				if (DrawingSurface.Children.Count > 0)
				{
					DrawingSurface.Children.Clear();
				}
				return;
			}

			var expectedShape = typeof(Ellipse);
			if (Model.DrawingState.Shape == ShapeType.Square)
			{
				expectedShape = typeof(Windows.UI.Xaml.Shapes.Rectangle);
			}

			var child = DrawingSurface.Children.FirstOrDefault() as Shape;
			if (child?.GetType() != expectedShape)
			{
				DrawingSurface.Children.Clear();
				if (Model.DrawingState.Shape == ShapeType.Square)
				{
					child = new Windows.UI.Xaml.Shapes.Rectangle();
				}
				else
				{
					child = new Ellipse();
				}
				DrawingSurface.Children.Add(child);
			}

			var brush = DrawingStateViewModel.SourceBrush;
			switch (Model.DrawingState.SurfaceType)
			{
				case CellState.Empty:
					brush = DrawingStateViewModel.EmptyBrush;
					break;
				case CellState.Wall:
					brush = DrawingStateViewModel.WallBrush;
					break;
			}
			child.Fill = brush;
			child.Stroke = DrawingStateViewModel.BrushBorder;
			child.StrokeThickness = 1;
			child.Width = Model.DrawingState.Size;
			child.Height = Model.DrawingState.Size;

			var point = e.GetCurrentPoint(DrawingSurface);
			var x = point.Position.X - Model.DrawingState.Size / 2;
			var y = point.Position.Y - Model.DrawingState.Size / 2;
			child.SetValue(Canvas.LeftProperty, x);
			child.SetValue(Canvas.TopProperty, y);

			child.Visibility = x > 0 && y > 0 && x < DrawingSurface.Width && y < DrawingSurface.Height ?
				Visibility.Visible : Visibility.Collapsed;

			if (!e.Pointer.IsInContact)
			{
				return;
			}

			var relativeSize = Model.DrawingState.Size / DrawingSurface.Width / 2;
			var relativeX = point.Position.X / DrawingSurface.Width;
			var relativeY = point.Position.Y / DrawingSurface.Height;
			
			IShape addedShape;
			if (Model.DrawingState.Shape == ShapeType.Circle)
			{
				addedShape = new Circle(new System.Drawing.PointF((float)relativeX, (float)relativeY), (float)relativeSize, Model.DrawingState.SurfaceType);
			}
			else
			{
				var left = relativeX - relativeSize;
				var top = relativeY - relativeSize;
				var right = relativeX + relativeSize;
				var bottom = relativeY + relativeSize;
				addedShape = new Logic.Rectangle(new System.Drawing.PointF((float)left, (float)top), new System.Drawing.PointF((float)right, (float)bottom), Model.DrawingState.SurfaceType);
			}

			Model.CurrentPreset.Add(addedShape);
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, HuygensPrincipleCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override HuygensPrincipleCanvasController CreateController(ISkiaCanvas canvas) =>
			new HuygensPrincipleCanvasController(canvas);
	}
}
