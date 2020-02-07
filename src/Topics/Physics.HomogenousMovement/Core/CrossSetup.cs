﻿using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Uap.Core;
using Physics.HomogenousMovement.Infrastructure.Topics;
using Physics.HomogenousMovement.Models;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared;
using Physics.Shared.Infrastructure.Setup;
using Physics.Shared.Infrastructure.Topics;
using Physics.Shared.ViewModels;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.Activation;

namespace Physics.HomogenousMovement.Core
{
    public class CrossSetup : DefaultAppSetup<CrossApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITopicNavigator, TopicNavigator>();
        }

        public override async void UpdateActivationArguments(IActivatedEventArgs e)
        {
            base.UpdateActivationArguments(e);
            if (e is IActivatedEventArgs activationArgs && activationArgs.Kind == ActivationKind.Protocol)
            {
                var eventArgs = activationArgs as ProtocolActivatedEventArgs;
                var uri = eventArgs.Uri;
                LaunchInfo launchInfo = LaunchUriManager.Deserialize<LaunchInfo>(uri);
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>()
                    .Navigate<MainViewModel, MainViewModel.NavigationModel>(new MainViewModel.NavigationModel() { Difficulty = DifficultyOption.Advanced, LaunchInfo = launchInfo });

            }
        }
    }
}
