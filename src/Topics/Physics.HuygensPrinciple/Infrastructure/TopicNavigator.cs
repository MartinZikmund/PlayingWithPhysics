using System;
using System.IO;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.HuygensPrinciple.ViewModels;
using Physics.SelfStudy;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Models.Navigation;
using Windows.ApplicationModel;

namespace Physics.HuygensPrinciple.Infrastructure
{
	public class TopicNavigator : ITopicConfiguration
	{
		private readonly IMvxNavigationService _navigationService;

		public TopicNavigator(IMvxNavigationService navigationService) =>
			_navigationService = navigationService;

		public bool HasAdvancedDifficulty => true;

		public bool HasStudyMode => true;

		public bool HasGame => true;

		public string GameNameOverride => Localizer.Instance.GetString("Demo");

		public int Id => 11;

		public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel, SimulationNavigationModel>(new SimulationNavigationModel { Difficulty = option });

		public async Task GoToGameAsync() => await _navigationService.Navigate<DemoViewModel, SimulationNavigationModel>(new SimulationNavigationModel { Difficulty = DifficultyOption.Advanced });

		public async Task GoToStudyModeAsync()
		{
			await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
		}

		public async Task OpenStudyTextAsync()
		{
		}
	}
}
