using Microsoft.Toolkit.Uwp.Helpers;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Physics.Shared.UI.Infrastructure
{
    public abstract class PhysicsAppBase<TMvxSetup, TApp> : MvxApplication<TMvxSetup, TApp>
        where TMvxSetup : DefaultAppSetup<TApp>, new()
        where TApp : class, IMvxApplication, new()
    {
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);
            TitleBarManager.Personalize((Color)Resources["AppTitleBarColor"], ElementTheme.Dark);
        }

        protected override Frame CreateFrame()
        {
            var frame = base.CreateFrame();
            frame.Background = new SolidColorBrush((Color)Resources["AppThemeColor"]);
            return frame;
        }
    }
}
