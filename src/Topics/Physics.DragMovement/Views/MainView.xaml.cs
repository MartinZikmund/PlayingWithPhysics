using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.DragMovement.ViewInteractions;
using Physics.DragMovement.ViewModels;
using Physics.Shared.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.DragMovement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : BaseView, IMainViewInteraction
    {
        //private HomogenousParticleCanvasController _canvasController;
        private CanvasAnimatedControl _animatedCanvas;

        public MainView()
        {
            this.InitializeComponent();
            this.Loaded += MainView_Loaded;
            DataContextChanged += MainView_DataContextChanged;
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.ThemeShadow"))
            {
                MenuPane.Translation = new System.Numerics.Vector3(0, 0, 16);
                ((ThemeShadow)MenuPane.Shadow).Receivers.Add(SecondPane);
            }
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {

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
                //Model.SetViewInteraction(this);
            }
        }

        public MainViewModel Model { get; private set; }

        //public HomogenousParticleCanvasController PrepareController(VelocityVariant variant)
        //{
        //    if (_animatedCanvas == null)
        //    {
        //        _animatedCanvas = new CanvasAnimatedControl();
        //        CanvasHolder.Children.Add(_animatedCanvas);
        //    }

        //    if (_canvasController == null)
        //    {
        //        _canvasController = new HomogenousParticleCanvasController(_animatedCanvas);
        //    }

        //    switch (variant)
        //    {
        //        case VelocityVariant.Zero:
        //            _canvasController.SetVariantRenderer(new ZeroVariantRenderer(_canvasController));
        //            break;
        //        case VelocityVariant.Parallel:
        //            _canvasController.SetVariantRenderer(new ParallelVariantRenderer(_canvasController));
        //            break;
        //        case VelocityVariant.Perpendicular:
        //            _canvasController.SetVariantRenderer(new PerpendicularVariantRenderer(_canvasController));
        //            break;
        //        case VelocityVariant.Radiation:
        //            _canvasController.SetVariantRenderer(new RadiationVariantRenderer(_canvasController));
        //            break;
        //    }
        //    return _canvasController;
        //}
    }
}
