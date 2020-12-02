using Physics.InclinedPlane.Rendering;
using Physics.InclinedPlane.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views.Interactions;
using Physics.Shared.Views;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Physics.InclinedPlane.Views
{
	public sealed partial class GameView : BaseView, ISimulationViewInteraction<InclinedPlaneSkiaController>
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
                _animatedCanvas.PointerMoved += _animatedCanvas_PointerMoved;
                _animatedCanvas.PointerExited += _animatedCanvas_PointerExited;
                _animatedCanvas.PointerPressed += _animatedCanvas_PointerPressed;
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

        private void _animatedCanvas_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Model.PreviewStone(null);
        }

        private void _animatedCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(_animatedCanvas);
            Model.CanvasTapped((float)point.Position.X);
        }

        private void _animatedCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(_animatedCanvas);
            Model.PreviewStone((float)point.Position.X);
        }

        private void _animatedCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasController?.Dispose();
        }
    }
}
