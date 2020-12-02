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
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Physics.Shared;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Platforms.Uap.Core;
using Physics.HomogenousMovement.Core;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Physics.HomogenousMovement.Models;
using MvvmCross;
using MvvmCross.Navigation;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Physics.Shared.UI.Services;
using Physics.Shared.UI.Infrastructure;

namespace Physics.HomogenousMovement
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("1d9d6ae2-9f90-4992-9ec2-a9811882f3dc",
                   typeof(Analytics), typeof(Crashes));
        }
    }

    public class HomogenousMovementApp : PhysicsAppBase<AppSetup, DefaultApp<AppStart>>
    {
    }
}
