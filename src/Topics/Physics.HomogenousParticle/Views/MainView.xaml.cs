using Physics.HomogenousParticle.ViewModels;
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
    public sealed partial class MainView : BaseView
    {
        public MainView()
        {
            this.InitializeComponent();
            DataContextChanged += MainView_DataContextChanged;
        }

        private void MainView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (MainViewModel)args.NewValue;
        }

        public MainViewModel Model { get; private set; }

        private void NumberBox_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            if (args.NewValue != 0 && args.NewValue < 2)
            {
                VelocityNumberBox.Value = args.OldValue;
            }
        }
    }
}
