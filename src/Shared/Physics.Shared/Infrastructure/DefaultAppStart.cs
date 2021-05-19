using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Services.AppList;
using Physics.Shared.ViewModels;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Infrastructure
{
	public class DefaultAppStart : MvxAppStart
    {
		private readonly IAppList _appList;

		public DefaultAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
			_appList = Mvx.IoCProvider.GetSingleton<IAppList>();
		}

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
			await _appList.InitializeAsync();
            await NavigationService.Navigate<MainMenuViewModel>();
        }
    }
}
