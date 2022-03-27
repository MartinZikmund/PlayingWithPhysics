using System;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Views;
using Physics.HomogenousMovement.Infrastructure.Topics;
using Physics.HomogenousMovement.Models;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.UI.Infrastructure;
using Physics.Shared.UI.Infrastructure.Setup;
using Physics.Shared.UI.Infrastructure.Topics;
using Windows.ApplicationModel.Activation;

namespace Physics.HomogenousMovement.Core
{
	public class AppSetup : DefaultAppSetup<DefaultApp<AppStart>>
	{
		protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
		{
			base.InitializeFirstChance(iocProvider);
			iocProvider.LazyConstructAndRegisterSingleton<ITopicConfiguration, TopicNavigator>();
		}

		public override async void UpdateActivationArguments(IActivatedEventArgs e)
		{
			base.UpdateActivationArguments(e);			
			if (e is IActivatedEventArgs activationArgs && activationArgs.Kind == ActivationKind.Protocol)
			{				
				try
				{
					var eventArgs = activationArgs as ProtocolActivatedEventArgs;
					var uri = eventArgs.Uri;
					LaunchInfo launchInfo = LaunchUriManager.Deserialize<LaunchInfo>(uri);
					await Mvx.IoCProvider.Resolve<IMvxNavigationService>()
						.Navigate<MainViewModel, SimulationNavigationModel>(new SimulationNavigationModel() { Difficulty = DifficultyOption.Advanced, LaunchInfo = launchInfo });
				}
				catch (Exception ex)
				{
					// Log.
				}
			}
		}
	}
}
