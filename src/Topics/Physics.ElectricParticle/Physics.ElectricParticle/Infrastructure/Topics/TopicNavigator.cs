using MvvmCross.Navigation;
using Physics.SelfStudy;
using Physics.Shared.UI.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.Infrastructure.Topics
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

        public async Task GoToStudyModeAsync()
        {
            await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
        }

        public async Task OpenStudyTextAsync()
        {
        }
    }
}
