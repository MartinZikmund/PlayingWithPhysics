using System;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Infrastructure.Interactions
{
    /// <summary>
    /// Represents an awaitable MvvmCross interaction request
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    public abstract class AsyncInteractionRequest<TResult>
    {
        private readonly TaskCompletionSource<TResult> _completionSource = new TaskCompletionSource<TResult>();

        public Task<TResult> GetResultAsync() => _completionSource.Task;

        public void SetResult(TResult result) => _completionSource.TrySetResult(result);

        public void SetException(Exception ex) => _completionSource.SetException(ex);
    }
}
