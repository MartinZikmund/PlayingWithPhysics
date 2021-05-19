using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.Shared.Logic;
using Physics.InclinedPlane.ViewModels;
using Physics.InclinedPlane.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.SelfStudy;
using Windows.ApplicationModel;
using System.IO;
using Physics.Shared.UI.ViewModels.Navigation;

namespace Physics.InclinedPlane.Infrastructure.Topics
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

		public int Id => 4;

		public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel, DifficultyNavigationModel>(new DifficultyNavigationModel { Difficulty = option });

        public async Task GoToGameAsync()
        {
            await _navigationService.Navigate<GameViewModel, GameViewModel.NavigationModel>(new GameViewModel.NavigationModel { Difficulty = DifficultyOption.Advanced });
        }

        public async Task GoToStudyModeAsync()
        {
            await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
        }

        public Task OpenStudyTextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
