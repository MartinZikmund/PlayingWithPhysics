using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousMovement.Rendering;
using Physics.HomogenousMovement.ViewInteractions;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.Helpers;
using Physics.Shared.Infrastructure.Topics;
using Physics.Shared.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.HomogenousMovement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameView : BaseView, IGameViewInteraction
    {
        private GamificationCanvasController _canvasController;
        private CanvasAnimatedControl _animatedCanvas;
        public GameView()
        {
            this.InitializeComponent();
            InkCanvas.InkPresenter.InputDeviceTypes =
                 Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                 Windows.UI.Core.CoreInputDeviceTypes.Pen |
                 Windows.UI.Core.CoreInputDeviceTypes.Touch;
            DataContextChanged += MainMenuView_DataContextChanged;
            //_canvasController = new GamificationCanvasController(AnimatedCanvas);
            this.Unloaded += MainView_Unloaded;
            StepSizeNumberBox.NumberFormatter = NumberBoxHelpers.SetupFromatting();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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
            Model = (GameViewModel)args.NewValue;
            Model.SetViewInteraction(this);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.Pause();
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.Play();
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;
        }
        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.Rewind(Model.StepSize);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.FastForward(Model.StepSize);
        }

        private void SpeedSldr_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_canvasController != null)
                _canvasController.SimulationTime.SimulationSpeed = (float)SpeedSldr.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SpeedSldr.Opacity == 0)
            {
                SpeedButtonStoryboardShow.Begin();
            }
            else
            {
                SpeedButtonStoryboardHide.Begin();
            }
        }

        private void Rewind_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.SimulationTime.Restart();
        }

        public GamificationCanvasController Initialize(DifficultyOption difficulty)
        {
            _animatedCanvas = new CanvasAnimatedControl();
            CanvasHolder.Children.Add(_animatedCanvas);
            _canvasController = new GamificationCanvasController(_animatedCanvas);
            return _canvasController;
        }
    }
}
