using System;
using System.Diagnostics;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views.Interactions;
using Physics.Shared.Views;
using SkiaSharp;
using SkiaSharp.Views.UWP;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Physics.Shared.UI.Views
{
	public abstract class BaseSkiaView<TViewModel, TController> : BaseView
		where TViewModel : class, IReceiveController<TController>
		where TController : IDisposable
	{
		private const string InkCanvasName = "InkCanvas";
		private const string MenuPaneName = "MenuPane";
		private const string SecondPaneName = "SecondPane";
		private const string CanvasHolderName = "CanvasHolder";

		private const int MenuShadowHeight = 16;

		private ISkiaCanvas _skiaCanvas;
		private TController _canvasController;

		public BaseSkiaView()
		{
			DataContextChanged += ViewContextChanged;
			Loaded += ViewLoaded;
			Unloaded += ViewUnloaded;			
		}

		private void ViewLoaded(object sender, RoutedEventArgs e)
		{
			SetupInkCanvas();
			SetupMenuShadow();
		}

		public TViewModel Model { get; private set; }

		private void ViewContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			var model = (TViewModel)args.NewValue;
			if (Model != model)
			{
				Model = model;
				var controller = PrepareController();
				Model.SetController(controller);
			}
		}

		private void SetupMenuShadow()
		{
			if (!ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
			{
				// Theme shadow API is not available on this device.
				return;
			}
			var menuPane = FindName(MenuPaneName) as FrameworkElement;
			var secondPane = FindName(SecondPaneName) as FrameworkElement;
			if (menuPane == null || secondPane == null)
			{
				Debug.WriteLine($"'{MenuPaneName}' or '{SecondPaneName}' does not exist. Can't set up theme shadow.");
				return;
			}

			menuPane.Translation = new System.Numerics.Vector3(0, 0, MenuShadowHeight);
			((ThemeShadow)menuPane.Shadow).Receivers.Add(secondPane);
		}

		private void SetupInkCanvas()
		{
			var inkCanvas = FindName(InkCanvasName) as InkCanvas;
			if (inkCanvas != null)
			{
				inkCanvas.InkPresenter.InputDeviceTypes =
					CoreInputDeviceTypes.Mouse |
					CoreInputDeviceTypes.Pen |
					CoreInputDeviceTypes.Touch;
			}
		}

		private TController PrepareController()
		{
			var canvasHolder = FindName(CanvasHolderName) as Grid;
			if (canvasHolder == null)
			{
				throw new InvalidOperationException($"The view must contain a Grid with name '{CanvasHolderName}' where the Skia canvas will be placed.");
			}

			if (_skiaCanvas == null)
			{
				_skiaCanvas = CreateSkiaCanvas();
				canvasHolder.Children.Add((UIElement)_skiaCanvas);
			}

			if (_canvasController == null)
			{
				_canvasController = CreateController(_skiaCanvas);
			}

			return _canvasController;
		}

		protected virtual ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		private void ViewUnloaded(object sender, RoutedEventArgs e)
		{
			_canvasController?.Dispose();
			_skiaCanvas = null;
		}

		protected abstract TController CreateController(ISkiaCanvas canvas);
	}
}
