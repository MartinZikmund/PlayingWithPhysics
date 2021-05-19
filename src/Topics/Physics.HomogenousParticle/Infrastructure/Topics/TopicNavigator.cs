using MvvmCross.Navigation;
using Physics.HomogenousParticle.ViewModels;
using System;
using System.Threading.Tasks;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.HomogenousParticle.Models;
using System.IO;
using Windows.ApplicationModel;
using Physics.SelfStudy;

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

		public string GameNameOverride => null;

		public int Id => 14;

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
