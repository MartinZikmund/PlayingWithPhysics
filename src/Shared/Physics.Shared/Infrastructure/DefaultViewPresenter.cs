using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;

namespace Physics.Shared.UI.Infrastructure
{
    public class DefaultViewPresenter : MvxWindowsViewPresenter
    {
        public DefaultViewPresenter(IMvxWindowsFrame rootFrame) : base(rootFrame)
        {
        }

        protected override void HandleBackButtonVisibility()
        {
            // Do not use title bar back button
        }
    }
}
