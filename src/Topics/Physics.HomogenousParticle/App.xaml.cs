﻿using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Platforms.Uap.Views;
using Physics.HomogenousParticle.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.HomogenousParticle
{    /// <summary>
     /// Provides application-specific behavior to supplement the default Application class.
     /// </summary>
    sealed partial class App : PhysicsApp
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("d6d547de-d761-45a8-8c8f-fd508f540d1e",
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
            frame.Transitions.Clear();
            frame.Background = new SolidColorBrush((Color)Resources["AppThemeColor"]);
            return frame;
        }

        private void SetupTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            var appColor = ColorHelper.ToColor("#0D2B4A");
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
