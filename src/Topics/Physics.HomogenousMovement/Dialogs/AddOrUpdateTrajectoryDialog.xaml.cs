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
using Physics.HomogenousMovement.ViewModels;

namespace Physics.HomogenousMovement.Dialogs
{
    public sealed partial class AddOrUpdateTrajectoryDialog : ContentDialog
    {
        public AddOrUpdateTrajectoryDialog(AddOrUpdateTrajectoryViewModel viewModel)
        {
            this.InitializeComponent();
            Model = viewModel;
        }

        public AddOrUpdateTrajectoryViewModel Model { get; }
    }
}
