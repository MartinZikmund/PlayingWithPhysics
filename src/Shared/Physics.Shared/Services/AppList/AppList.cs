using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Physics.Shared.UI.Models;

namespace Physics.Shared.UI.Services.AppList
{
	public class AppList : IAppList
	{
		private static AppListItem[] _apps = null;

		public IReadOnlyList<AppListItem> Apps => _apps;

		public AppListItem GetAppById(int id)
		{
			if (id == 9999999)
			{
				return new AppListItem()
				{
					Id = 9999999,
					Name = "AppTemplate",
					PackageId = "id",
					StoreId = "storeId"
				};
			}

			var app = Apps
				.Where(app => app.Id == id)
				.FirstOrDefault();
			if (app == null)
			{
				throw new InvalidOperationException(
					"App with ID does not exist in the app list." +
					"Add it to Physics.Shared.UI/Assets/AppList.json first.");
			}

			return app;
		}

		public async Task InitializeAsync()
		{
			if (_apps == null)
			{
				using var stream = GetAppListResourceStream();
				using var streamReader = new StreamReader(stream);
				var appListContents = await streamReader.ReadToEndAsync();
				_apps = JsonConvert.DeserializeObject<AppListItem[]>(appListContents);
			}
		}

		private Stream GetAppListResourceStream()
		{
			var assembly = typeof(AppList).Assembly;
			var manifestResourceNames = assembly.GetManifestResourceNames();
			var item = manifestResourceNames
				.Where(n => n.EndsWith("AppList.json", StringComparison.OrdinalIgnoreCase))
				.SingleOrDefault();
			if (item == null)
			{
				throw new InvalidOperationException("Unique AppList.json manifest resource could not be found.");
			}

			return assembly.GetManifestResourceStream(item);
		}
	}
}
