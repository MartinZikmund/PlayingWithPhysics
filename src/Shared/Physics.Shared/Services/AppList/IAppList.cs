using System.Collections.Generic;
using System.Threading.Tasks;
using Physics.Shared.UI.Models;

namespace Physics.Shared.UI.Services.AppList
{
	public interface IAppList
    {
		Task InitializeAsync();

		IReadOnlyList<AppListItem> Apps { get; }

		public AppListItem GetAppById(int id);
    }
}
