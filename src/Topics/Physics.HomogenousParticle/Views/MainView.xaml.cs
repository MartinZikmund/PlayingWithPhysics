﻿using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.HomogenousParticle.Rendering;
using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.ViewInteractions;
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
    public sealed partial class MainView : BaseView, IMainViewInteraction
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

        }

        private void MainView_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasController?.Dispose();
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

        public HomogenousParticleCanvasControllerBase PrepareController(VelocityVariant variant)
        {
            if (_animatedCanvas == null)
            {
                _animatedCanvas = new CanvasAnimatedControl();
                CanvasHolder.Children.Add(_animatedCanvas);
            }

            _canvasController?.Dispose();

            switch (variant)
            {
                case VelocityVariant.Zero:
                    _canvasController = new ZeroVariantCanvasController(_animatedCanvas);
                    break;
                case VelocityVariant.Parallel:
                    
                    break;
                case VelocityVariant.Perpendicular:
                    break;
                case VelocityVariant.Greek:
                    break;
            }
            return _canvasController;
        }
    }
}
