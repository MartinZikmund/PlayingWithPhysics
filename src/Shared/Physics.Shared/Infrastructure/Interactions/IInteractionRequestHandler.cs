using System.Threading.Tasks;
using MvvmCross.Base;

namespace Physics.Shared.UI.Infrastructure.Interactions
{
    /// <summary>
    /// Represents parameterless MvvmCross interaction handler
    /// </summary>
    public interface IInteractionRequestHandler
    {
        Task HandleAsync();
    }

    /// <summary>
    /// Represents a general MvvmCross interaction handler
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IInteractionRequestHandler<TArgs>
    {
        /// <summary>
        /// Handles the interaction
        /// </summary>
        /// <param name="args">Interaction arguments</param>
        Task HandleAsync(TArgs args);

        void OnRequested(object sender, MvxValueEventArgs<TArgs> eventArgs);
    }
}
