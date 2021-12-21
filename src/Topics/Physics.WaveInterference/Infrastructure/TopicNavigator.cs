using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.SelfStudy;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.WaveInterference.ViewModels;
using Windows.ApplicationModel;

namespace Physics.WaveInterference.Infrastructure
{
	class TopicNavigator : ITopicConfiguration
	{
		private readonly IMvxNavigationService _navigationService;

		public TopicNavigator(IMvxNavigationService navigationService) =>
			_navigationService = navigationService;

		public bool HasAdvancedDifficulty => true;

		public bool HasStudyMode => true;

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
			await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
		}

		public Task OpenStudyTextAsync()
		{
			throw new NotImplementedException();
		}
	}
}
