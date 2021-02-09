using System;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.Shared.UI.Views
{
	public sealed partial class AppShell : Page
	{
		public AppShell()
		{
			this.InitializeComponent();
		}

		public static AppShell GetForCurrentView() => Window.Current.Content as AppShell;

		public Frame AppFrame => MainFrame;

		internal void ShowInfoBar(InfoBar infoBar)
		{
			InfoBarContainer.Children.Add(infoBar);
			infoBar.IsOpen = true;
			infoBar.Closed += InfoBar_Closed;
		}

		private void InfoBar_Closed(InfoBar sender, InfoBarClosedEventArgs args)
		{
			InfoBarContainer.Children.Remove(sender);
		}
	}
}
