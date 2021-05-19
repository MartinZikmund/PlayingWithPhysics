using MvvmCross.Commands;
using MvvmCross.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.AppList;
using Physics.Shared.UI.Models;
using MvvmCross.Navigation;
using Physics.Shared.UI.ViewModels;

namespace Physics.Shared.ViewModels
{
	public class MainMenuViewModel : MvxViewModel
	{
		private readonly ITopicConfiguration _topicNavigator;
		private readonly IAppList _appList;
		private readonly IMvxNavigationService _navigationService;
		private ICommand _goToEasyCommand;
		private ICommand _goToAdvancedCommand;
		private ICommand _goToStudyModeCommand;
		private ICommand _gameModeCommand;
		private ICommand _aboutAppCommand;
		private MvxAsyncCommand _moreAppsCommand;

		public MainMenuViewModel(ITopicConfiguration topicNavigator, IAppList appList, IMvxNavigationService navigationService)
		{
			_topicNavigator = topicNavigator ?? throw new ArgumentNullException(nameof(topicNavigator));
			_appList = appList ?? throw new ArgumentNullException(nameof(appList));
			_navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
			App = new AppListItemViewModel(_appList.GetAppById(topicNavigator.Id));
		}

		public AppListItemViewModel App { get; }

		public Uri MenuImageUri => new Uri($"ms-appx:///Physics.Shared.UI/Assets/Logos/{App.Name}.png");

		public bool HasGame => _topicNavigator.HasGame;

		public string GameName => _topicNavigator.GameNameOverride ?? Localizer.Instance["Game"];

		public bool HasStudyMode => _topicNavigator.HasStudyMode && IsCurrentCultureCzech;

		public bool HasAdvancedDifficulty => _topicNavigator.HasAdvancedDifficulty;

		public ICommand GoToEasyCommand => _goToEasyCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Easy);

		public ICommand GoToAdvancedCommand => _goToAdvancedCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Advanced);

		public ICommand GameModeCommand => _gameModeCommand ??= new MvxAsyncCommand(_topicNavigator.GoToGameAsync);

		public ICommand GoToStudyModeCommand => _goToStudyModeCommand ??= new MvxAsyncCommand(_topicNavigator.GoToStudyModeAsync);

		public ICommand MoreAppsCommand => _moreAppsCommand ??= new MvxAsyncCommand(GoToMoreAppsAsync);

		private async Task GoToMoreAppsAsync() => await _navigationService.Navigate<AppListViewModel>();

		private bool IsCurrentCultureCzech =>
#if DEBUG
			true;
#else
            CultureInfo.CurrentUICulture
                .TwoLetterISOLanguageName
                .StartsWith("cs", StringComparison.InvariantCultureIgnoreCase);
#endif

		private ICommand CreateTopicDifficultyCommand(DifficultyOption difficulty)
		{
			return new MvxAsyncCommand(async () =>
			{
				await _topicNavigator.GoToDifficultyAsync(difficulty);
			});
		}
	}
}
