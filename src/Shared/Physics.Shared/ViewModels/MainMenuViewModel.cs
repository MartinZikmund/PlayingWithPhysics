using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Physics.Shared.Infrastructure.Topics;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace Physics.Shared.ViewModels
{
    public class MainMenuViewModel : MvxViewModel
    {
        private readonly ITopicNavigator _topicNavigator;

        private ICommand _goToEasyCommand;
        private ICommand _goToAdvancedCommand;
        private ICommand _openStudyTextCommand;
        private ICommand _gameModeCommand;

        public MainMenuViewModel(ITopicNavigator topicNavigator) => _topicNavigator = topicNavigator;

        public ICommand GoToEasyCommand => _goToEasyCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Easy);

        public ICommand GoToAdvancedCommand => _goToAdvancedCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Advanced);

        public ICommand GameModeCommand => _gameModeCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Game);

        public ICommand OpenStudyTextCommand =>
            _openStudyTextCommand ??= new MvxAsyncCommand(_topicNavigator.OpenStudyTextAsync);

        private ICommand CreateTopicDifficultyCommand(DifficultyOption difficulty) =>
            new MvxAsyncCommand(() => _topicNavigator.GoToDifficultyAsync(difficulty));

        public string Version
        {
            get
            {
                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;

                return "Verze: " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }
    }
}
