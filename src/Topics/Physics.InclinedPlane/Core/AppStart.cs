using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Physics.InclinedPlane.ViewModels;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Core
{
    public class AppStart : MvxAppStart
    {

        public AppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            await NavigationService.Navigate<MainMenuViewModel>();
        }
    }
}
