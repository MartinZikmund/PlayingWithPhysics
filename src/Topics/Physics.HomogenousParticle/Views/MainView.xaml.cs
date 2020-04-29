using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Rendering;
using Physics.HomogenousParticle.ViewModels;
using Physics.Shared.Helpers;
using Physics.Shared.Views;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.HomogenousParticle.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : BaseView
    {
        private HomogenousParticleCanvasControllerBase _canvasController;
        private CanvasAnimatedControl _animatedCanvas;

        public MainView()
        {
            this.InitializeComponent();
            this.Loaded += MainView_Loaded;
            DataContextChanged += MainView_DataContextChanged;
            StepSizeNumberBox.SetupFormatting();
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void MainView_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasController?.Dispose();
            _animatedCanvas?.RemoveFromVisualTree();
            _animatedCanvas = null;
        }

        private void MainView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (MainViewModel)args.NewValue;
        }

        public MainViewModel Model { get; private set; }

        public HomogenousParticleCanvasControllerBase Initialize()
        {
            _animatedCanvas = new CanvasAnimatedControl();
            CanvasHolder.Children.Add(_animatedCanvas);
            _canvasController = new ZeroVariantCanvasController(_animatedCanvas);
            return _canvasController;
        }
    }
}
