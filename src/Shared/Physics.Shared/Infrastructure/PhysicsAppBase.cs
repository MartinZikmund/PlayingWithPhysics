using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using MvvmCross.Commands;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Physics.Shared.Services.Store;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services;
using Physics.Shared.UI.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Physics.Shared.UI.Infrastructure
{
	public abstract class PhysicsAppBase<TMvxSetup, TApp> : MvxApplication<TMvxSetup, TApp>
		where TMvxSetup : DefaultAppSetup<TApp>, new()
		where TApp : class, IMvxApplication, new()
	{
		protected override void OnWindowCreated(WindowCreatedEventArgs args)
		{
			base.OnWindowCreated(args);
			TitleBarManager.Personalize((Color)Resources["AppTitleBarColor"]);
		}

		protected override Frame CreateFrame()
		{
			throw new InvalidOperationException("This should not be called, as the app is based on an app shell.");
		}

		protected override Frame InitializeFrame(IActivatedEventArgs activationArgs)
		{
			AppShell appShell = Window.Current.Content as AppShell;
			if (appShell == null)
			{
				appShell = new AppShell();
				appShell.AppFrame.Transitions.Clear();
				appShell.AppFrame.Background = new SolidColorBrush((Color)Resources["AppThemeColor"]);
				appShell.AppFrame.NavigationFailed += AppFrame_NavigationFailed;
				Window.Current.Content = appShell;
			}

			if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
			{
				OnResumeFromTerminateState();
			}

			RootFrame = appShell.AppFrame;
			return appShell.AppFrame;
		}

		private void AppFrame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
		{
			Debug.WriteLine("Navigation failed: " + e.Exception);
		}

		protected override async void OnLaunched(LaunchActivatedEventArgs activationArgs)
		{
			base.OnLaunched(activationArgs);

			await TryUpdateAsync();
		}

		private async Task TryUpdateAsync()
		{
			try
			{
				if (AppUpdater.IsSupported())
				{
					var updater = AppUpdater.GetDefault();

					if (await updater.AreUpdatesAvailableAsync())
					{
						var appShell = AppShell.GetForCurrentView();
						// Show info bar to ask user whether to update
						var infoBar = new InfoBar()
						{
							Title = Localizer.Instance.GetString("UpdateAvailable_Title"),
							Message = Localizer.Instance.GetString("UpdateAvailable_Description"),
							ActionButton = new Button()
							{
								Content = Localizer.Instance.GetString("UpdateAvailable_Button"),
								Command = new MvxAsyncCommand(DownloadUpdateAsync),
							}
						};
						infoBar.ActionButton.Click += (s, e) => infoBar.IsOpen = false;
						appShell.ShowInfoBar(infoBar);
					}
				}
			}
			catch (Exception)
			{
				Debug.WriteLine("Update check failed due to network issues or because we are debugging.");
			}
		}

		private async Task DownloadUpdateAsync()
		{
			var appUpdater = AppUpdater.GetDefault();
			var result = await appUpdater.DownloadUpdatesAsync();
			if (result.UpdatesAvailable && result.State == Windows.Services.Store.StorePackageUpdateState.Completed)
			{
				var appShell = AppShell.GetForCurrentView();
				// Show info bar to ask user whether to update
				var infoBar = new InfoBar()
				{
					Title = Localizer.Instance.GetString("UpdateReady_Title"),
					Message = Localizer.Instance.GetString("UpdateReady_Description"),
					ActionButton = new Button()
					{
						Content = Localizer.Instance.GetString("UpdateReady_Button"),
						Command = new MvxAsyncCommand(UpdateNowAsync)
					}
				};
				infoBar.ActionButton.Click += (s, e) => infoBar.IsOpen = false;
				appShell.ShowInfoBar(infoBar);
			}
		}

		private async Task UpdateNowAsync()
		{
			var appUpdater = AppUpdater.GetDefault();
			await appUpdater.InstallUpdatesAsync();
		}
	}
}
