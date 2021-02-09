#pragma warning disable UWP001 // Platform-specific
using Windows.Services.Store;

namespace Physics.Shared.Services.Store
{
	public class AppUpdaterResult
    {
		public AppUpdaterResult(bool updatesAvailable, StorePackageUpdateState state)
		{
			UpdatesAvailable = updatesAvailable;
			State = state;
		}

		public bool UpdatesAvailable { get; set; }

		public StorePackageUpdateState State { get; set; }
	}
}
#pragma warning restore UWP001 // Platform-specific
