﻿using Microsoft.UI.Xaml.Controls;
using Physics.SelfStudy.Editor.ViewModels;
using Physics.SelfStudy.Models;
using Physics.Shared.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Physics.SelfStudy.Editor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        private static Lazy<AppShell> _instance = new Lazy<AppShell>(() => new AppShell());

        private AppShell()
        {
            this.InitializeComponent();
            TitleBarManager.Personalize((Color)Application.Current.Resources["AppTitleBarColor"]);
            TitleBarManager.SetExtendIntoView(true, TitleBar);            
            DataContext = ViewModel;
            ViewModel = new AppShellViewModel();
        }

        public static AppShell Instance => _instance.Value;

        public AppShellViewModel ViewModel { get; }
    }
}
