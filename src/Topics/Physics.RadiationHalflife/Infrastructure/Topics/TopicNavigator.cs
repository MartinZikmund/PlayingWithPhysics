using System;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.ApplicationModel;
using System.IO;
using Physics.RadiationHalflife.ViewModels;
using Physics.Shared.UI.ViewModels.Navigation;
using Physics.Shared.UI.Localization;

namespace Physics.RadiationHalflife.Infrastructure.Topics
{
	class TopicNavigator : ITopicConfiguration
	{
		private readonly IMvxNavigationService _navigationService;

		public TopicNavigator(IMvxNavigationService navigationService) =>
			_navigationService = navigationService;

		public bool HasAdvancedDifficulty => true;

		public bool HasStudyMode => false;

		public bool HasGame => true;

		public string GameNameOverride => Localizer.Instance["Demo"];

		public int Id => 17;

		public async Task GoToDifficultyAsync(DifficultyOption option) =>
			await _navigationService.Navigate<MainViewModel, DifficultyNavigationModel>(new DifficultyNavigationModel { Difficulty = option });

		public async Task GoToGameAsync() => await _navigationService.Navigate<DemoViewModel, DifficultyNavigationModel>(new DifficultyNavigationModel(DifficultyOption.Advanced));
		//public async Task GoToGameAsync() => throw new NotImplementedException();

		public Task GoToStudyModeAsync()
		{
			throw new NotSupportedException("Study mode not yet supported.");
			//await StudyModeManager.OpenStudyModeAsync(new Uri("ms-appx:///Assets/StudyMode/index.json"), Path.Combine(Package.Current.InstalledLocation.Path, "Assets/StudyMode"));
		}
	}
}
