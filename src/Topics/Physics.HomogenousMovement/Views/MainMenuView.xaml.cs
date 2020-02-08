using MvvmCross.Platforms.Uap.Views;
using Physics.Shared.ViewModels;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.HomogenousMovement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenuView : MvxWindowsPage
    {
        public MainMenuView()
        {
            this.InitializeComponent();
            VideoBackground.MediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            VideoBackground.MediaPlayer.IsLoopingEnabled = true;
            DataContextChanged += MainMenuView_DataContextChanged;
            RootGrid.Opacity = 0;
            RootGrid.OpacityTransition = new Windows.UI.Xaml.ScalarTransition()
            {
                Duration = TimeSpan.FromSeconds(0.3)
            };

            Splash.OpacityTransition = new Windows.UI.Xaml.ScalarTransition()
            {
                Duration = TimeSpan.FromSeconds(0.3)
            };
        }

        private async void MediaPlayer_MediaOpened(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            await this.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    RootGrid.Opacity = 1;
                    Splash.Opacity = 0;
                });
        }

        public MainMenuViewModel Model { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                GC.Collect(); //clean up memory between topic launches
            }
        }

        private void MainMenuView_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            Model = (MainMenuViewModel)args.NewValue;
        }
    }
}
