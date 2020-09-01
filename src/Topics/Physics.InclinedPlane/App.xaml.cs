using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Platforms.Uap.Views;
using Physics.InclinedPlane.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.InclinedPlane
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : PhysicsApp
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("a6b20ac1-6216-4862-9c2a-dccf5b6df820",
                   typeof(Analytics), typeof(Crashes));
        }   
    }
    public class PhysicsApp : MvxApplication<CrossSetup, Core.CrossApp>
    {
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);
            SetupTitleBar();
        }

        protected override Frame CreateFrame()
        {
            var frame = base.CreateFrame();
            frame.Background = new SolidColorBrush((Color)Resources["AppThemeColor"]);
            return frame;
        }

        private void SetupTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            var appColor = ColorHelper.ToColor("#9e3f5a");
            var inactiveForeground = Colors.LightGray;
            titleBar.BackgroundColor = appColor;
            titleBar.ButtonBackgroundColor = appColor;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = appColor;
            titleBar.InactiveForegroundColor = inactiveForeground;
            titleBar.ButtonInactiveBackgroundColor = appColor;
            titleBar.ButtonInactiveForegroundColor = inactiveForeground;
        }
    }
}
