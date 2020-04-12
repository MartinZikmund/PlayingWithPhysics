using MvvmCross.Platforms.Uap.Views;
using Physics.Shared.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using Windows.Media.Core;
using Windows.UI.Xaml;
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
         
            DataContextChanged += MainMenuView_DataContextChanged;

            // hide study text in non-CZ
            if (!CultureInfo.CurrentUICulture.IetfLanguageTag.StartsWith("cs"))
            {
                StudyTextButton.Visibility = Visibility.Collapsed;
            }

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
