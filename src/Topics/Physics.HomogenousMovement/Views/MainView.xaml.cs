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
using Microsoft.Graphics.Canvas.Brushes;
using Physics.Shared.Views;
using Physics.HomogenousMovement.Rendering;
using Windows.Globalization.NumberFormatting;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;
using Physics.HomogenousMovement.ViewInteractions;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.UI.Xaml.Media;
using Windows.Foundation.Metadata;

namespace Physics.HomogenousMovement
{
    public sealed partial class MainView : BaseView, IMainViewInteraction
    {
        private HomogenousMovementCanvasController _canvasController;
        private CanvasAnimatedControl _animatedCanvas;
        public MainView()
        {
            this.InitializeComponent();
            InkCanvas.InkPresenter.InputDeviceTypes =
                 Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                 Windows.UI.Core.CoreInputDeviceTypes.Pen |
                 Windows.UI.Core.CoreInputDeviceTypes.Touch;
            DataContextChanged += MainMenuView_DataContextChanged;
            //_canvasController = new GamificationCanvasController(AnimatedCanvas);
            this.Unloaded += MainView_Unloaded;
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
            {
                MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
                ((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);
            }
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

        public MainViewModel Model { get; private set; }

        private void MainMenuView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var model = (MainViewModel)args.NewValue;
            if (Model != model)
            {
                Model = model;
                Model.SetViewInteraction(this);
            }
        }

        public HomogenousMovementCanvasController Initialize(DifficultyOption difficulty)
        {
            _animatedCanvas = new CanvasAnimatedControl();
            CanvasHolder.Children.Add(_animatedCanvas);
            _canvasController = new HomogenousMovementCanvasController(_animatedCanvas);            
            return _canvasController;
        }
    }
}