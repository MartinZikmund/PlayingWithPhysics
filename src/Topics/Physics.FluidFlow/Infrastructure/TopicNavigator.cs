using System;
using System.IO;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using Physics.SelfStudy;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.FluidFlow.ViewModels;
using Windows.ApplicationModel;
using Physics.Shared.UI.Localization;

namespace Physics.FluidFlow.Infrastructure
{
	public class TopicNavigator : ITopicConfiguration
	{
		private readonly IMvxNavigationService _navigationService;

		public TopicNavigator(IMvxNavigationService navigationService) =>
			_navigationService = navigationService;

		public bool HasAdvancedDifficulty => true;

		public bool HasStudyMode => true;

		public bool HasGame => true;

		public string GameNameOverride => Localizer.Instance.GetString("SomethingMore");

		public int Id => 15;

		public async Task GoToDifficultyAsync(DifficultyOption option) =>
			await _navigationService.Navigate<MainViewModel, SimulationNavigationModel>(new SimulationNavigationModel { Difficulty = option });

		public async Task GoToGameAsync() =>
			await StudyModeManager.OpenStudyModeAsync(
				new Uri("ms-appx:///Assets/Demo/index.json"),
				Path.Combine(Package.Current.InstalledLocation.Path, "Assets/Demo"),
				GameNameOverride);

		public async Task GoToStudyModeAsync() =>
			await StudyModeManager.OpenStudyModeAsync(
				new Uri("ms-appx:///Assets/StudyMode/index.json"),
				Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
	}
}
