using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Physics.Shared.ViewModels;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Infrastructure
{
	public class DefaultAppStart : MvxAppStart
    {
        public DefaultAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            await NavigationService.Navigate<MainMenuViewModel>();
        }
    }
}
