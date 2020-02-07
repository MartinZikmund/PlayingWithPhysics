using MvvmCross.Navigation;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.System;

namespace Physics.HomogenousMovement.Infrastructure.Topics
{
    class TopicNavigator : ITopicNavigator
    {
        private readonly IMvxNavigationService _navigationService;

        public TopicNavigator(IMvxNavigationService navigationService) =>
            _navigationService = navigationService;

        public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel,MainViewModel.NavigationModel>(new MainViewModel.NavigationModel { Difficulty = option });

        public async Task OpenStudyTextAsync()
        {
            await Launcher.LaunchFileAsync(
                await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appx:///Assets/Texts/StudijniText.pdf")));
        }
    }
}
