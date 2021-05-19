using System.Linq;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models;
using Physics.Shared.UI.Services.AppList;
using Physics.Shared.ViewModels;

namespace Physics.Shared.UI.ViewModels
{
	public class AppListViewModel : ViewModelBase
    {
		private readonly IAppList _appList;
		private readonly ITopicConfiguration _topicConfiguration;

		public AppListViewModel(IAppList appList, ITopicConfiguration topicConfiguration)
		{
			_appList = appList ?? throw new System.ArgumentNullException(nameof(appList));
			_topicConfiguration = topicConfiguration ?? throw new System.ArgumentNullException(nameof(topicConfiguration));
			Apps = _appList.Apps
				.Where(app => app.Id != _topicConfiguration.Id)
				.Select(app => new AppListItemViewModel(app))
				.ToArray();
		}

		public AppListItemViewModel[] Apps { get; }
    }
}
