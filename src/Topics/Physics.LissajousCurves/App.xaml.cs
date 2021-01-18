using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Physics.LissajousCurves.Core;
using Physics.Shared.UI.Infrastructure;
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

namespace Physics.LissajousCurves
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : PhysicsApp
	{
		public App()
		{
			this.InitializeComponent();
			AppCenter.Start("7648efa6-50ff-4a96-b01b-94e0d8b11dca",
				   typeof(Analytics), typeof(Crashes));

		}

	}
	public class PhysicsApp : PhysicsAppBase<AppSetup, DefaultApp>
	{
	}
}
