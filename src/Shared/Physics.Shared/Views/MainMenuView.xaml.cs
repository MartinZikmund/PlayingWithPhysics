using Physics.Shared.UI.Controls;
using Physics.Shared.UI.Localization;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Physics.Shared.Views
{
    public sealed partial class MainMenuView : BaseView
    {
        public MainMenuView()
        {
            InitializeComponent();

            DataContextChanged += MainMenuView_DataContextChanged;
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

        private async void AboutAppButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();                        
            dialog.Content = new AboutApp();
            dialog.IsPrimaryButtonEnabled = true;
            dialog.PrimaryButtonText = Localizer.Instance["Close"];
            dialog.PrimaryButtonClick += (s, e) =>
            {
                dialog.Hide();
            };
            await dialog.ShowAsync();
        }
    }
}
