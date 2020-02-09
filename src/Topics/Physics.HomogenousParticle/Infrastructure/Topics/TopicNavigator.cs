using MvvmCross.Navigation;
using Physics.HomogenousParticle.ViewModels;
using Physics.Shared.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.Infrastructure.Topics
{
    public class TopicNavigator : ITopicNavigator
    {
        private readonly IMvxNavigationService _navigationService;

        public TopicNavigator(IMvxNavigationService navigationService) =>
            _navigationService = navigationService;

        public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel>();

        public Task OpenStudyTextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
