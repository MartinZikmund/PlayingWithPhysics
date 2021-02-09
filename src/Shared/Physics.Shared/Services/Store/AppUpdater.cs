#pragma warning disable UWP001 // Platform-specific
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Services.Store;

namespace Physics.Shared.Services.Store
{
	public class AppUpdater
	{
		private readonly StoreContext _context = null;
		private IReadOnlyList<StorePackageUpdate> _availableUpdates;

		public static bool IsSupported() => ApiInformation.IsTypePresent("Windows.Services.Store.StoreContext");

		private AppUpdater()
		{
			_context = StoreContext.GetDefault();
		}

		public static AppUpdater GetDefault()
		{
			if (!IsSupported())
			{
				throw new InvalidOperationException();
			}
			return new AppUpdater();
		}

		public async Task<bool> AreUpdatesAvailableAsync()
		{
//#if DEBUG
//			return true;
//#endif
			_availableUpdates = await _context.GetAppAndOptionalStorePackageUpdatesAsync();
			return _availableUpdates.Any();
		}

		public async Task<AppUpdaterResult> DownloadUpdatesAsync()
		{
//#if DEBUG
//			await Task.Delay(4000);
//			return new AppUpdaterResult(true, StorePackageUpdateState.Completed);
//#endif
			// Get the updates that are available.
			var storePackageUpdates = await _context.GetAppAndOptionalStorePackageUpdatesAsync();

			if (storePackageUpdates.Count > 0)
			{
				var downloadResult = await _context.RequestDownloadStorePackageUpdatesAsync(storePackageUpdates);

				return new AppUpdaterResult(true, downloadResult.OverallState);
			}

			return new AppUpdaterResult(false, StorePackageUpdateState.Completed);
		}

		public async Task<AppUpdaterResult> InstallUpdatesAsync()
		{
//#if DEBUG
//			await Task.Delay(4000);
//			return new AppUpdaterResult(true, StorePackageUpdateState.Completed);
//#endif

			// Get the updates that are available.
			var storePackageUpdates = await _context.GetAppAndOptionalStorePackageUpdatesAsync();

			if (storePackageUpdates.Count > 0)
			{
				// Start the silent installation of the packages. Because the packages have already
				// been downloaded in the previous method, the following line of code just installs
				// the downloaded packages.
				var downloadResult = await _context.RequestDownloadAndInstallStorePackageUpdatesAsync(storePackageUpdates);

				return new AppUpdaterResult(true, downloadResult.OverallState);
			}

			return new AppUpdaterResult(false, StorePackageUpdateState.Completed);
		}
	}
}
#pragma warning restore UWP001 // Platform-specific
