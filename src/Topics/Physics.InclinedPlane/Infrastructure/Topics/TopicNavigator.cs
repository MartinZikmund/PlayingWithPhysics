﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.Shared.Logic;
using Physics.InclinedPlane.ViewModels;
using Physics.InclinedPlane.Views;
using Physics.Shared.UI.Infrastructure.Topics;

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

        public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel, MainViewModel.NavigationModel>(new MainViewModel.NavigationModel { Difficulty = option });

        public async Task GoToGameAsync()
        {
            await _navigationService.Navigate<GameViewModel, GameViewModel.NavigationModel>(new GameViewModel.NavigationModel { Difficulty = DifficultyOption.Advanced });
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
