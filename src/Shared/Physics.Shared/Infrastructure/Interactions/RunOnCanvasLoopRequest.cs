using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.Infrastructure.Interactions
{
    public class RunOnCanvasLoopRequest : AsyncInteractionRequest<bool>
    {
        public RunOnCanvasLoopRequest(Action action) => Action = action;

        public Action Action { get; }
    }
}
