using MvvmCross.Navigation;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.Infrastructure.Topics;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Physics.HomogenousMovement.Models;
using Physics.SelfStudy;

namespace Physics.HomogenousMovement.Infrastructure.Topics
{
    class TopicNavigator : ITopicConfiguration
    {
        private readonly IMvxNavigationService _navigationService;

        public TopicNavigator(IMvxNavigationService navigationService) =>
            _navigationService = navigationService;

        public bool HasAdvancedDifficulty => true;

        public bool HasStudyMode => true;

        public bool HasGame => true;

        public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel, SimulationNavigationModel>(new SimulationNavigationModel { Difficulty = option });

        public async Task GoToGameAsync()
        {
            await _navigationService.Navigate<GameViewModel, SimulationNavigationModel>(
                new SimulationNavigationModel() { Difficulty = DifficultyOption.Advanced });
        }

        public async Task GoToStudyModeAsync()
        {
            await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"));
        }
    }
}
