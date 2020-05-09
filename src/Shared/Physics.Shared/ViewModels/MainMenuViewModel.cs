﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.Shared.ViewModels
{
    public class MainMenuViewModel : MvxViewModel
    {
        private readonly ITopicConfiguration _topicNavigator;

        private ICommand _goToEasyCommand;
        private ICommand _goToAdvancedCommand;
        private ICommand _goToStudyModeCommand;
        private ICommand _gameModeCommand;

        public MainMenuViewModel(ITopicConfiguration topicNavigator) => _topicNavigator = topicNavigator;

        public bool HasGame => _topicNavigator.HasGame;

        public bool HasStudyMode => _topicNavigator.HasStudyMode && IsCurrentCultureCzech;

        public bool HasAdvancedDifficulty => _topicNavigator.HasAdvancedDifficulty;

        public ICommand GoToEasyCommand => _goToEasyCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Easy);

        public ICommand GoToAdvancedCommand => _goToAdvancedCommand ??= CreateTopicDifficultyCommand(DifficultyOption.Advanced);

        public ICommand GameModeCommand => _gameModeCommand ??= new MvxAsyncCommand(_topicNavigator.GoToGameAsync);

        public ICommand GoToStudyModeCommand =>
            _goToStudyModeCommand ??= new MvxAsyncCommand(_topicNavigator.GoToStudyModeAsync);

        private bool IsCurrentCultureCzech =>
#if DEBUG
            true;
#else
            CultureInfo.CurrentUICulture
                .TwoLetterISOLanguageName
                .StartsWith("cs", StringComparison.InvariantCultureIgnoreCase);
#endif

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
