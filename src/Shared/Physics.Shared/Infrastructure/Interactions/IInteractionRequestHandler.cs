using MvvmCross.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Infrastructure.Interactions
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
