using System;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.DragMovement.Rendering;
using Physics.DragMovement.ViewInteractions;
using Physics.DragMovement.ViewModels;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.Views;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Physics.DragMovement.Views
{
	public sealed partial class GameView : BaseView, IGameViewInteraction
	{
		private GamificationCanvasController _canvasController;
		private CanvasAnimatedControl _animatedCanvas;

		public GameView()
		{
			InitializeComponent();
			InkCanvas.InkPresenter.InputDeviceTypes =
				 Windows.UI.Core.CoreInputDeviceTypes.Mouse |
				 Windows.UI.Core.CoreInputDeviceTypes.Pen |
				 Windows.UI.Core.CoreInputDeviceTypes.Touch;
			DataContextChanged += MainMenuView_DataContextChanged;
			//_canvasController = new GamificationCanvasController(AnimatedCanvas);
			SecondPane.Loaded += SecondPane_Loaded;
			Unloaded += MainView_Unloaded;
			SecondPane.SizeChanged += SecondPane_SizeChanged;
			SetupNumberBoxFormattings();
			if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
			{
				MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
				((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);

				CanvasWrapper.Translation = new System.Numerics.Vector3(0, 0, 8);
				((ThemeShadow)CanvasWrapper.Shadow).Receivers.Add(Letterbox);
			}
		}

		private void SecondPane_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateCanvasSize();
		}

		private void SecondPane_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateCanvasSize();
		}

		private void UpdateCanvasSize()
		{
			var aspectRatio = SecondPane.ActualWidth / SecondPane.ActualHeight;
			if (aspectRatio < 1)
			{
				CanvasWrapper.Width = SecondPane.ActualWidth;
				CanvasWrapper.Height = SecondPane.ActualWidth;
			}
			else
			{
				CanvasWrapper.Width = SecondPane.ActualWidth;
				CanvasWrapper.Height = SecondPane.ActualHeight;
			}
		}

		private void SetupNumberBoxFormattings()
		{
			CargoMassNumberBox.SetupFormatting(increment: 15, smallChange: 15);
			//GravityNumberBox.SetupFormatting(fractionDigits: 2);
			//V0NumberBox.SetupFormatting(fractionDigits: 2);
			//AngleNumberBox.SetupFormatting(fractionDigits: 2);
			//GravityNumberBox.SmallChange = 0.1;
		}

		private void MainView_Unloaded(object sender, RoutedEventArgs e)
		{
			_canvasController?.Dispose();
			_animatedCanvas?.RemoveFromVisualTree();
			_animatedCanvas = null;
		}

		public GameViewModel Model { get; private set; }

		private void MainMenuView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			var model = (GameViewModel)args.NewValue;
			if (Model != model)
			{
				Model = model;
				Model.SetViewInteraction(this);
			}
		}

		public GamificationCanvasController Initialize(DifficultyOption difficulty, ISoundPlayer soundPlayer)
		{
			_animatedCanvas = new CanvasAnimatedControl();
			CanvasHolder.Children.Add(_animatedCanvas);
			_canvasController = new GamificationCanvasController(_animatedCanvas, soundPlayer);
			return _canvasController;
		}
	}
}
