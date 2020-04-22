using Microsoft.UI.Xaml.Controls;
using Physics.SelfStudy.Editor.ViewModels;
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
            DataContext = ViewModel;
            ViewModel = new AppShellViewModel();
        }

        public static AppShell Instance => _instance.Value;

        public Microsoft.UI.Xaml.Controls.TreeView TreeView => Tree;

        public AppShellViewModel ViewModel { get; }

        private void Tree_ItemInvoked(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs args)
        {
            ViewModel.Workspace.CurrentProject.UpdateSelection();
        }
    }
}
