using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using Physics.Shared.UI.Localization;
using Windows.System;

namespace Physics.Shared.UI.Models
{
	public class AppListItemViewModel
	{
		private readonly AppListItem _app;
		private MvxAsyncCommand _openCommand;

		public AppListItemViewModel(AppListItem app)
		{
			_app = app;
		}

		public int Id => _app.Id;

		public string Name => _app.Name;

		public string ShortName => Localizer.Instance.GetString($"AppName_{_app.Name}");

		public Uri MenuImageUri => new Uri($"ms-appx:///Physics.Shared.UI/Assets/Logos/{_app.Name}.jpg");

		public ICommand OpenCommand => _openCommand ??= new MvxAsyncCommand(OpenAsync);

		private async Task OpenAsync()
		{
			var queryResult = await Launcher.QueryUriSupportAsync(
				new Uri(GetAppUriScheme()),
				LaunchQuerySupportType.Uri,
				_app.PackageId);

			if (queryResult == LaunchQuerySupportStatus.AppNotInstalled)
			{
				// Launch store
				await Launcher.LaunchUriAsync(new Uri($"https://www.microsoft.com/store/apps/{_app.StoreId}"));
			}
			else
			{
				// Open app
				await Launcher.LaunchUriAsync(
					new Uri($"{GetAppUriScheme()}/home", UriKind.Absolute),
					new LauncherOptions()
					{
						FallbackUri = new Uri($"https://www.microsoft.com/store/apps/{_app.StoreId}")
					});
			}
		}

		private string GetAppUriScheme()
		{
			var appName = string.IsNullOrEmpty(_app.ShortenedName) ? _app.Name : _app.ShortenedName;
			return $"playingwithphysics-{appName.ToLowerInvariant()}:";
		}
	}
}
