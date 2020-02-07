using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Infrastructure.Interactions
{
    /// <summary>
    /// Represents an awaitable MvvmCross interaction
    /// </summary>
    /// <typeparam name="TRequest">Request type (deriving from AwaitableInteractionRequest)</typeparam>
    /// <typeparam name="TResult">Request result type</typeparam>
    public class AsyncInteraction<TRequest, TResult> : MvxInteraction<TRequest>
        where TRequest : AsyncInteractionRequest<TResult>
    {
        public Task<TResult> RaiseAsync(TRequest request)
        {
            Raise(request);
            return request.GetResultAsync();
        }
    }
}
