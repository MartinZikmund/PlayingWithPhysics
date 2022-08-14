using System;
using System.IO;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using Physics.SelfStudy;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.CyclicProcesses.ViewModels;
using Windows.ApplicationModel;
using Physics.Shared.UI.Localization;

namespace Physics.CyclicProcesses.Infrastructure
{
	public class TopicNavigator : ITopicConfiguration
	{
		private readonly IMvxNavigationService _navigationService;

		public TopicNavigator(IMvxNavigationService navigationService) =>
			_navigationService = navigationService;

		public bool HasAdvancedDifficulty => true;

		public bool HasStudyMode => true;

		public bool HasGame => true;

		public string GameNameOverride => Localizer.Instance["Demo"];

		public int Id => 12;

		public async Task GoToDifficultyAsync(DifficultyOption option)
		{
			if (option == DifficultyOption.Easy)
			{
				await _navigationService.Navigate<EasyVariantViewModel>();
			}
			else
			{
				await _navigationService.Navigate<AdvancedVariantViewModel, SimulationNavigationModel>(new SimulationNavigationModel { Difficulty = option });
			}
		}

		public async Task GoToGameAsync() => await _navigationService.Navigate<DemoVariantViewModel>();

		public async Task GoToStudyModeAsync()
		{
			await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
		}

		public async Task OpenStudyTextAsync()
		{
		}
	}
}
