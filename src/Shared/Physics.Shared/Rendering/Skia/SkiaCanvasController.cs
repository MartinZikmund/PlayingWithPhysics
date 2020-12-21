using SkiaSharp;
using SkiaSharp.Views.UWP;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;

namespace Physics.Shared.UI.Rendering.Skia
{
    public abstract class SkiaCanvasController : ICanvasController, IDisposable
    {
        private bool _simluationInitiated = false;

        protected ISkiaCanvas _canvas;

        protected SkiaCanvasController(ISkiaCanvas canvasAnimatedControl)
        {
            SimulationTime = new SimulationTime(this);
            _canvas = canvasAnimatedControl ?? throw new ArgumentNullException(nameof(canvasAnimatedControl));
			_canvas.Initialized += Initialized;
            _canvas.Draw += Draw;
            _canvas.Update += CanvasUpdate;
        }

		public bool IsPaused { get; private set; }

        public SimulationTime SimulationTime { get; }

        public TimeSpan? MaxTime { get; set; }

		public void Play()
		{
			IsPaused = false;
			SimulationTime.Start();
		}

		public void Pause()
		{
			IsPaused = true;
			SimulationTime.Pause();
		}

		public void Rewind(float time)
        {
            SimulationTime.Rewind(time);
        }

        public void FastForward(float time)
        {
            SimulationTime.FastForward(time);
        }

		public virtual void Initialized(ISkiaCanvas sender, SKSurface args) { }

        public abstract void Draw(ISkiaCanvas sender, SKSurface args);

        public abstract void Update(ISkiaCanvas sender);

        public async Task RunOnGameLoopAsync(Action action)
        {
            await _canvas.RunOnRenderThreadAsync(action);
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
			IsPaused = false;
        }

        private void CanvasUpdate(ISkiaCanvas sender, EventArgs e)
        {
            if (!IsPaused && _simluationInitiated)
            {
                SimulationTime.OnUpdateStarting();
            }
            _simluationInitiated = true;

            Update(sender);
        }

		protected SKBitmap LoadImageFromPackage(string relativePath)
		{
			var path = Path.Combine(Package.Current.InstalledLocation.Path, relativePath);
			return SKBitmap.Decode(path);
		}
    }
}
