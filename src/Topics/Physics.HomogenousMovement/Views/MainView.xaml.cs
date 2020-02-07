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
        private ThrowingCanvasController _canvasController;
        public MainView()
        {
            this.InitializeComponent();
            _canvasController = new ThrowingCanvasController(AnimatedCanvas);
            DataContextChanged += MainMenuView_DataContextChanged;
            this.Unloaded += MainView_Unloaded;
            SetupNumberBoxFormattings();
        }

        private void SetupFromatting(NumberBox numberBox, int decimalPlaces)
        {
            var rounder = new IncrementNumberRounder();
            rounder.Increment = 0.1;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            var formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = decimalPlaces;
            formatter.NumberRounder = rounder;
            numberBox.NumberFormatter = formatter;
        }

        private void SetupNumberBoxFormattings()
        {
            SetupFromatting(StartXNumberBox, 2);
            SetupFromatting(StartYNumberBox, 2);
            SetupFromatting(GravityNumberBox, 2);
            SetupFromatting(MassNumberBox, 2);
            SetupFromatting(V0NumberBox, 2);
            SetupFromatting(AngleNumberBox, 2);
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

        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var ap = new ButtonAutomationPeer(DrawButton);
                var ip = ap.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                ip?.Invoke();
            }
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
    }
}