using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using Windows.ApplicationModel;
using System.IO;

namespace Physics.Shared.UI.Rendering
{
	public abstract class BaseCanvasController : ICanvasController
	{
		private bool _simluationInitiated = false;

		protected CanvasAnimatedControl _canvasAnimatedControl;
		private bool _isPaused;

		public event EventHandler PlayStateChanged;

		protected BaseCanvasController(CanvasAnimatedControl canvasAnimatedControl)
		{
			SimulationTime = new SimulationTime(this);
			_canvasAnimatedControl = canvasAnimatedControl ?? throw new ArgumentNullException(nameof(canvasAnimatedControl));
			_canvasAnimatedControl.Draw += Draw;
			_canvasAnimatedControl.Update += CanvasUpdate;
			_canvasAnimatedControl.CreateResources += CreateResources;
		}

		public bool IsPaused
		{
			get => _isPaused;
			private set
			{
				if (_isPaused != value)
				{
					_isPaused = value;
					PlayStateChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

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

		protected void Reset()
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

		protected async Task<CanvasBitmap> LoadImageFromPackageAsync(string relativePath)
		{
			var path = Path.Combine(Package.Current.InstalledLocation.Path, relativePath);
			return await CanvasBitmap.LoadAsync(_canvasAnimatedControl, path);
		}
	}
}
