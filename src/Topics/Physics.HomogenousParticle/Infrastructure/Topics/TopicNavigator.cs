using MvvmCross.Navigation;
using Physics.HomogenousParticle.ViewModels;
using System;
using System.Threading.Tasks;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HomogenousParticle.Infrastructure.Topics
{
    public class TopicNavigator : ITopicConfiguration
    {
        private readonly IMvxNavigationService _navigationService;

        public TopicNavigator(IMvxNavigationService navigationService) =>
            _navigationService = navigationService;

        public bool HasAdvancedDifficulty => true;

        public bool HasStudyMode => true;

        public bool HasGame => false;

        public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel, SimulationNavigationModel>(new SimulationNavigationModel { Difficulty = option });

        public Task GoToGameAsync()
        {
            throw new NotImplementedException();
        }

        public Task GoToStudyModeAsync()
        {
            throw new NotImplementedException();
        }

        public Task OpenStudyTextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
