using System;

namespace Physics.Shared.UI.Infrastructure.Interactions
{
    public class RunOnCanvasLoopRequest : AsyncInteractionRequest<bool>
    {
        public RunOnCanvasLoopRequest(Action action) => Action = action;

        public Action Action { get; }
    }
}
