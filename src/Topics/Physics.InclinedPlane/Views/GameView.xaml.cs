using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.InclinedPlane.Rendering;
using Physics.InclinedPlane.ViewInteractions;
using Physics.InclinedPlane.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.Views;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.InclinedPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameView : BaseView, IMainViewInteraction
    {
        private SkiaCanvas _animatedCanvas;
        private InclinedPlaneSkiaController _canvasController;

        public GameView()
        {
            this.InitializeComponent();            
            DataContextChanged += GameView_DataContextChanged;
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
            {
                MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
                ((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);
            }
        }

        private void GameView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var model = (GameViewModel)args.NewValue;
            if (Model != model)
            {
                Model = model;
                Model.SetViewInteraction(this);
            }
        }

        public GameViewModel Model { get; private set; }

        private void GameView_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasController?.Dispose();
            _animatedCanvas = null;
        }

        public InclinedPlaneSkiaController PrepareController()
        {
            if (_animatedCanvas == null)
            {
                _animatedCanvas = new SkiaCanvas();
                _animatedCanvas.Unloaded += _animatedCanvas_Unloaded;
                CanvasHolder.Children.Add(_animatedCanvas);
            }

            if (_canvasController == null)
            {
                _canvasController = new InclinedPlaneSkiaController(_animatedCanvas);
            }

            _canvasController.SetVariantRenderer(new GameRenderer(_canvasController));
            return _canvasController;
        }

        private void _animatedCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasController?.Dispose();
        }
    }
}
