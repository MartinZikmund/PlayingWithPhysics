using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.Controls;
using Microsoft.Graphics.Canvas.Brushes;
using Physics.Shared.Infrastructure.Interactions;
using Physics.Shared.Views;
using Physics.HomogenousMovement.Rendering;
using Windows.Globalization.NumberFormatting;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace Physics.HomogenousMovement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : BaseView
    {
        private MotioningCanvasController _canvasController;
        public MainView()
        {
            this.InitializeComponent();
            InkCanvas.InkPresenter.InputDeviceTypes =
                 Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                 Windows.UI.Core.CoreInputDeviceTypes.Pen |
                 Windows.UI.Core.CoreInputDeviceTypes.Touch;
            _canvasController = new MotioningCanvasController(AnimatedCanvas);
            DataContextChanged += MainMenuView_DataContextChanged;
            this.Unloaded += MainView_Unloaded;
        }

        


        private void MainView_Unloaded(object sender, RoutedEventArgs e)
        {            
            Model.SetCanvasController(null);
            _canvasController.Dispose();
            AnimatedCanvas.RemoveFromVisualTree();
            AnimatedCanvas = null;
        }

        public MainViewModel Model { get; private set; }

        private void MainMenuView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (MainViewModel)args.NewValue;
            Model.SetCanvasController(_canvasController);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.Pause();
            PlayButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.Play();
            PlayButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
        }
        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.Rewind(0.1f);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            _canvasController.FastForward(0.1f);
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
    }
}