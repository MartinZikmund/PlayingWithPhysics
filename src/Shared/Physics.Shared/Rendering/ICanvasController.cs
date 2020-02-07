using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Physics.Shared.Rendering
{
    public interface ICanvasController : IDisposable
    {
        Task RunOnGameLoopAsync(DispatchedHandler agileCallback);
    }
}
