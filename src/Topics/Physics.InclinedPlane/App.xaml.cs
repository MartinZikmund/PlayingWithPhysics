using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Platforms.Uap.Views;
using Physics.InclinedPlane.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Physics.InclinedPlane
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : PhysicsApp
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("1d9d6ae2-9f90-4992-9ec2-a9811882f3dc",
                   typeof(Analytics), typeof(Crashes));
        }   
    }
    public class PhysicsApp : MvxApplication<CrossSetup, Core.CrossApp>
    {

    }
}
