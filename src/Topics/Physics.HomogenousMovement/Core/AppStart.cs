using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Physics.HomogenousMovement.Models;
using Physics.HomogenousMovement.ViewModels;
using Physics.Shared.ViewModels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.HomogenousMovement.Core
{
    public class AppStart : MvxAppStart
    {

        public AppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            await NavigationService.Navigate<MainMenuViewModel>();
            if (hint is IActivatedEventArgs activationArgs && activationArgs.Kind == ActivationKind.Protocol)
            {
                var eventArgs = activationArgs as ProtocolActivatedEventArgs;
                var uri = eventArgs.Uri;
                var launchInfo = LaunchUriManager.Deserialize<LaunchInfo>(uri);                
                await NavigationService.Navigate<MainViewModel, SimulationNavigationModel>(new SimulationNavigationModel() { Difficulty = DifficultyOption.Advanced, LaunchInfo = launchInfo });
            }
        }
    }
}
