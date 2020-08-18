using SkiaSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Physics.Shared.UI.Rendering.Skia
{
    public abstract class SkiaCanvasController : IRenderingPlayback, IDisposable
    {
        private bool _simluationInitiated = false;

        protected SkiaCanvas _canvas;

        protected SkiaCanvasController(SkiaCanvas canvasAnimatedControl)
        {
            _canvas = canvasAnimatedControl ?? throw new ArgumentNullException(nameof(canvasAnimatedControl));
            _canvas.Draw += Draw;
            _canvas.Update += CanvasUpdate;
        }

        public bool IsPaused { get; private set; }

        public SimulationTime SimulationTime { get; } = new SimulationTime();

        public void Play() => IsPaused = false;

        public void Pause() => IsPaused = true;

        public void Rewind(float time)
        {
            SimulationTime.Rewind(time);
        }

        public void FastForward(float time)
        {
            SimulationTime.FastForward(time);
        }

        public abstract void Draw(SkiaCanvas sender, SKSurface args);

        public abstract void Update(SkiaCanvas sender);

        public async Task RunOnGameLoopAsync(DispatchedHandler agileCallback)
        {
            await _canvas.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, agileCallback);
        }

        public virtual void Dispose()
        {
            if (_canvas != null)
            {
                _canvas.Draw -= Draw;
                _canvas.Update -= CanvasUpdate;                
            }
            _canvas = null;
            Debug.WriteLine("Base canvas controller has been disposed.");
        }

        protected void Restart()
        {
            SimulationTime.Restart();
        }

        private void CanvasUpdate(SkiaCanvas sender, EventArgs e)
        {
            if (!IsPaused && _simluationInitiated)
            {
                SimulationTime.OnUpdateStarting();
            }
            _simluationInitiated = true;

            Update(sender);
        }        
    }
}
