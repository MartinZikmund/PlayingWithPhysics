using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using SkiaSharp;
using SKNativeView = SkiaSharp.Views.UWP.SKSwapChainPanel;

namespace Physics.Shared.UI.Rendering.Skia
{
	public class NonUiSkiaCanvas : SKNativeView, ISkiaCanvas
	{
		private ConcurrentQueue<Action> _waitingActions = new ConcurrentQueue<Action>();

		public NonUiSkiaCanvas()
		{
			System.Diagnostics.Debug.WriteLine("Creating skia canvas");

			EnableRenderLoop = true;
			DrawInBackground = true;
			PaintSurface += OnNativeControlInitialized;

			System.Diagnostics.Debug.WriteLine("Finished skia canvas");
		}

		public async Task RunOnRenderThreadAsync(Action action)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();
			_waitingActions.Enqueue(() =>
			{
				action();
				taskCompletionSource.SetResult(true);
			});
			await taskCompletionSource.Task;
		}

		private void OnNativeControlInitialized(object sender, SkiaSharp.Views.UWP.SKPaintGLSurfaceEventArgs e)
		{
			PaintSurface -= OnNativeControlInitialized;
			Initialized?.Invoke(this, e.Surface);
			this.PaintSurface += OnRendering;
		}

		private void OnRendering(object sender, SkiaSharp.Views.UWP.SKPaintGLSurfaceEventArgs e)
		{
			// Run waiting actions on rendering thread
			while(_waitingActions.TryDequeue(out var action))
			{
				action();
			}

			Update?.Invoke(this, EventArgs.Empty);
			var surface = e.Surface;
			var canvas = surface.Canvas;

			var worldMatrix = SKMatrix.CreateIdentity();
			// Apply DPI scaling.
			SKMatrix.Concat(ref worldMatrix, worldMatrix, SKMatrix.CreateScale((float)ContentsScale, (float)ContentsScale));

			canvas.ResetMatrix();
			canvas.SetMatrix(worldMatrix);
			Draw?.Invoke(this, e.Surface);
		}		

		public SKSize ScaledSize => new SKSize(CanvasSize.Width / (float)ContentsScale, CanvasSize.Height / (float)ContentsScale);

		public SKSize NativeSize => CanvasSize;

		public event SkiaEventHandler<SKSurface> Initialized;
		public event SkiaEventHandler<EventArgs> Update;
		public event SkiaEventHandler<SKSurface> Draw;
	}
}
