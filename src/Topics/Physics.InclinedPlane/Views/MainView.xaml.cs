using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.InclinedPlane.Rendering;
using Physics.InclinedPlane.ViewInteractions;
using Physics.InclinedPlane.ViewModels;
using Physics.Shared.Views;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.InclinedPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : BaseView, IMainViewInteraction
    {
        private CanvasAnimatedControl _animatedCanvas;
        private InclinedPlaneCanvasController _canvasController;

        public MainView()
        {
            this.InitializeComponent();
            InkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen |
                Windows.UI.Core.CoreInputDeviceTypes.Touch;
            DataContextChanged += MainView_DataContextChanged;
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
            {
                MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
                ((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);
            }
        }

        private void MainView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var model = (MainViewModel)args.NewValue;
            if (Model != model)
            {
                Model = model;
                Model.SetViewInteraction(this);
            }
        }

        public MainViewModel Model { get; private set; }

        private void MainView_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasController?.Dispose();
            _animatedCanvas?.RemoveFromVisualTree();
            _animatedCanvas = null;
        }

        public InclinedPlaneCanvasController PrepareController()
        {
            if (_animatedCanvas == null)
            {
                _animatedCanvas = new CanvasAnimatedControl();
                CanvasHolder.Children.Add(_animatedCanvas);
            }

            if (_canvasController == null)
            {
                _canvasController = new InclinedPlaneCanvasController(_animatedCanvas);
            }

            _canvasController.SetVariantRenderer(new SimulationRenderer(_canvasController));
            return _canvasController;
        }
    }
}
