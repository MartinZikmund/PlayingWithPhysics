using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.DragMovement.Rendering;
using Physics.DragMovement.ViewInteractions;
using Physics.DragMovement.ViewModels;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.Views;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Physics.DragMovement.Views
{
    public sealed partial class MainView : BaseView, IMainViewInteraction
	{
        private DragMovementCanvasController _canvasController;
        private CanvasAnimatedControl _animatedCanvas;

        public MainView()
        {
            InitializeComponent();
            DataContextChanged += MainView_DataContextChanged;
			Unloaded += MainView_Unloaded;
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
            {
                MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
                ((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);
            }
        }

		private void MainView_Unloaded(object sender, RoutedEventArgs e)
        {
            //_canvasController?.Dispose();
            _animatedCanvas?.RemoveFromVisualTree();
            _animatedCanvas = null;
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

        public DragMovementCanvasController Initialize(DifficultyOption difficulty)
        {
            _animatedCanvas = new CanvasAnimatedControl();
            CanvasHolder.Children.Add(_animatedCanvas);
            _canvasController = new DragMovementCanvasController(_animatedCanvas);
            return _canvasController;
        }
    }
}
