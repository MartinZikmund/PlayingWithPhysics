using System;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.ApplicationModel;
using System.IO;
using Physics.CompoundOscillations.ViewModels;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.SelfStudy;

namespace Physics.CompoundOscillations.Infrastructure.Topics
{
	class TopicNavigator : ITopicConfiguration
    {
        private readonly IMvxNavigationService _navigationService;

        public TopicNavigator(IMvxNavigationService navigationService) => 
            _navigationService = navigationService;

        public bool HasAdvancedDifficulty => true;

        public bool HasStudyMode => true;

        public bool HasGame => true;

		public string GameNameOverride => null;

		public int Id => 7;

		public async Task GoToDifficultyAsync(DifficultyOption option) =>
			await _navigationService.Navigate<MainViewModel, DifficultyNavigationModel>(new DifficultyNavigationModel { Difficulty = option });

		public async Task GoToGameAsync() =>
			await _navigationService.Navigate<GameViewModel, DifficultyNavigationModel>(new DifficultyNavigationModel(DifficultyOption.Advanced));

		public async Task GoToStudyModeAsync()
        {
			await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
		}
    }
}
