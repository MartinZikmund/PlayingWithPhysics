using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Physics.Shared.UI.Rendering
{
    public interface ICanvasController : IRenderingPlayback, IDisposable
    {
        Task RunOnGameLoopAsync(DispatchedHandler agileCallback);
    }
}
