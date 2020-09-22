using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Physics.Shared.UI.Rendering
{
    public abstract class BaseCanvasController : ICanvasController
    {
        private bool _simluationInitiated = false;

        protected CanvasAnimatedControl _canvasAnimatedControl;

        protected BaseCanvasController(CanvasAnimatedControl canvasAnimatedControl)
        {
            SimulationTime = new SimulationTime(this);
            _canvasAnimatedControl = canvasAnimatedControl ?? throw new ArgumentNullException(nameof(canvasAnimatedControl));
            _canvasAnimatedControl.Draw += Draw;
            _canvasAnimatedControl.Update += CanvasUpdate;
            _canvasAnimatedControl.CreateResources += CreateResources;
        }      
        
        public bool IsPaused { get; private set; }

        public SimulationTime SimulationTime { get; }

        public TimeSpan? MaxTime { get; set; }

        public void Play() => IsPaused = false;

        public void Pause() => IsPaused = true;        

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

        protected void Restart()
        {
            SimulationTime.Restart();
        }

        private void CanvasUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (!IsPaused && _simluationInitiated)
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
    }
}
