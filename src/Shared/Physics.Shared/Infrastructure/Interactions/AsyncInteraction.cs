using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace Physics.Shared.UI.Infrastructure.Interactions
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
