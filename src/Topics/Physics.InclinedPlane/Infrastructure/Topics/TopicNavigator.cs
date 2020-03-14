﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.Shared.Logic;
using Physics.Shared.Infrastructure.Topics;
using Physics.InclinedPlane.ViewModels;
using Physics.InclinedPlane.Views;

namespace Physics.InclinedPlane.Infrastructure.Topics
{
    class TopicNavigator : ITopicNavigator
    {
        private readonly IMvxNavigationService _navigationService;

        public TopicNavigator(IMvxNavigationService navigationService) => 
            _navigationService = navigationService;

        public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel>();

        public Task GoToGameAsync()
        {
            throw new NotImplementedException();
        }

        public Task OpenStudyTextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
