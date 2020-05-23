using Microsoft.Toolkit.Uwp.Helpers;
using Physics.Shared.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;

namespace Physics.Shared.UI.Services
{
    public static class TitleBarManager
    {
        public static void Personalize(Color backgroundColor)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var appColor = backgroundColor;

            var inactiveForeground = Colors.LightGray;
            var activeForeground = Colors.White;

            ElementTheme requestedTheme = backgroundColor.GetPerceivedLuminance() < 0.5 ? ElementTheme.Dark : ElementTheme.Light;

            if (requestedTheme == ElementTheme.Light)
            {
                inactiveForeground = Colors.DimGray;
                activeForeground = Colors.Black;
            }

            titleBar.InactiveForegroundColor = inactiveForeground;
            titleBar.ButtonInactiveForegroundColor = inactiveForeground;
            titleBar.ForegroundColor = activeForeground;
            titleBar.ButtonForegroundColor = activeForeground;
            titleBar.BackgroundColor = appColor;
            titleBar.ButtonBackgroundColor = appColor;
            titleBar.InactiveBackgroundColor = appColor;
            titleBar.ButtonInactiveBackgroundColor = appColor;
        }

        public static void Personalize(AppWindowTitleBar titleBar, Color backgroundColor)
        {
            var appColor = backgroundColor;

            var inactiveForeground = Colors.LightGray;
            var activeForeground = Colors.White;

            ElementTheme requestedTheme = backgroundColor.GetPerceivedLuminance() < 0.5 ? ElementTheme.Dark : ElementTheme.Light;

            if (requestedTheme == ElementTheme.Light)
            {
                inactiveForeground = Colors.DimGray;
                activeForeground = Colors.Black;
            }

            titleBar.InactiveForegroundColor = inactiveForeground;
            titleBar.ButtonInactiveForegroundColor = inactiveForeground;
            titleBar.ForegroundColor = activeForeground;
            titleBar.ButtonForegroundColor = activeForeground;
            titleBar.BackgroundColor = appColor;
            titleBar.ButtonBackgroundColor = appColor;
            titleBar.InactiveBackgroundColor = appColor;
            titleBar.ButtonInactiveBackgroundColor = appColor;
        }

        public static void SetExtendIntoView(bool extend)
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = extend;
        }
    }
}
