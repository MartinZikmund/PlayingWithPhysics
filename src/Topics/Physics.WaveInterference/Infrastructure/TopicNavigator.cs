using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.WaveInterference.ViewModels;

namespace Physics.WaveInterference.Infrastructure
{
	class TopicNavigator : ITopicConfiguration
	{
		private readonly IMvxNavigationService _navigationService;

		public TopicNavigator(IMvxNavigationService navigationService) =>
			_navigationService = navigationService;

		public bool HasAdvancedDifficulty => true;

		public bool HasStudyMode => false;

		public bool HasGame => false;

		public string GameNameOverride => null;

		public int Id => 8;

		public async Task GoToDifficultyAsync(DifficultyOption option) => await _navigationService.Navigate<MainViewModel, DifficultyNavigationModel>(new DifficultyNavigationModel { Difficulty = option });

		public async Task GoToGameAsync()
		{
			throw new NotImplementedException();
		}

		public async Task GoToStudyModeAsync()
		{
			throw new NotImplementedException();
		}

		public Task OpenStudyTextAsync()
		{
			throw new NotImplementedException();
		}
	}
}
