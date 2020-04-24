using Physics.SelfStudy.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Physics.SelfStudy.Views
{
    public sealed partial class SelfStudyView : Page
    {
        public SelfStudyView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        public SelfStudyViewModel ViewModel { get; } = new SelfStudyViewModel();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadAsync((Uri)e.Parameter);
        }
    }
}
