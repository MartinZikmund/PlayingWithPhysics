using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Physics.Shared.Rendering
{
    public abstract class BaseCanvasController : ICanvasController
    {
        private bool _simluationInitiated = false;
        private bool _simulationPaused = false;

        protected CanvasAnimatedControl _canvasAnimatedControl;

        protected BaseCanvasController(CanvasAnimatedControl canvasAnimatedControl)
        {
            _canvasAnimatedControl = canvasAnimatedControl ?? throw new ArgumentNullException(nameof(canvasAnimatedControl));
            _canvasAnimatedControl.Draw += Draw;
            _canvasAnimatedControl.Update += CanvasUpdate;
            _canvasAnimatedControl.CreateResources += CreateResources;
        }        

        public SimulationTime SimulationTime { get; } = new SimulationTime();

        public void Play() => _simulationPaused = false;

        public void Pause() => _simulationPaused = true;        

        public void Rewind(float time)
        {
            RunOnGameLoopAsync(() => { SimulationTime.Rewind(time); });
        }

        public void FastForward(float time)
        {
            RunOnGameLoopAsync(() => { SimulationTime.FastForward(time); });
        }

        public abstract Task CreateResourcesAsync(CanvasAnimatedControl sender);

        public abstract void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args);

        public abstract void Update(ICanvasAnimatedControl sender);

        public Task RunOnGameLoopAsync(DispatchedHandler agileCallback) => _canvasAnimatedControl.RunOnGameLoopThreadAsync(agileCallback).AsTask();

        protected void Restart()
        {
            SimulationTime.Restart();
        }

        private void CanvasUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (!_simulationPaused && _simluationInitiated)
            {
                SimulationTime.UpdateFromCanvas(args.Timing);
            }
            _simluationInitiated = true;

            Update(sender);
        }

        private void CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        public virtual void Dispose()
        {
            if (_canvasAnimatedControl != null)
            {
                _canvasAnimatedControl.Draw -= Draw;
                _canvasAnimatedControl.Update -= CanvasUpdate;
                _canvasAnimatedControl.CreateResources -= CreateResources;
            }
            _canvasAnimatedControl = null;
            Debug.WriteLine("Base canvas controller has been disposed.");
        }
    }
}
