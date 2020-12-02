using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousMovement.Rendering;
using Physics.HomogenousMovement.ViewInteractions;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.Views;
using Physics.Shared.Services.Sounds;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.Foundation.Metadata;

namespace Physics.HomogenousMovement.Views
{
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
            SetupNumberBoxFormattings();
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
            {
                MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
                ((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);
            }
        }

        private void SetupNumberBoxFormattings()
        {
            //GravityNumberBox.SetupFormatting(fractionDigits: 2);
            V0NumberBox.SetupFormatting(fractionDigits: 2);
            AngleNumberBox.SetupFormatting(fractionDigits: 2);
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
