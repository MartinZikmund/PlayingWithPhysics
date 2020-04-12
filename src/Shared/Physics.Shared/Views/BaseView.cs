using MvvmCross.Base;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Physics.Shared.Infrastructure.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Physics.Shared.Views
{
    public abstract class BaseView : MvxWindowsPage
    {
        protected void SetInteraction<TRequest>(ref IMvxInteraction<TRequest> field, IMvxInteraction<TRequest> newValue, EventHandler<MvxValueEventArgs<TRequest>> handler)
        {
            if (field != null)
            {
                field.Requested -= handler;
            }

            field = newValue;
            field.Requested += handler;
        }

        protected void SetAsyncInteraction<TRequest,TResult>(ref AsyncInteraction<TRequest, TResult> field, AsyncInteraction<TRequest,TResult> newValue, IInteractionRequestHandler<TRequest> handler)
            where TRequest : AsyncInteractionRequest<TResult>
        {
            if (field != null)
            {
                field.Requested -= handler.OnRequested;
            }

            field = newValue;
            field.Requested += handler.OnRequested;
        }

        ~BaseView()
        {
            Debug.WriteLine("Page has been destroyed" + GetType().Name);
        }
    }
}
