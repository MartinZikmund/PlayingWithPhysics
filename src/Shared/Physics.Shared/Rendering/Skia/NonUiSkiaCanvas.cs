using System;
using SkiaSharp;
using SKNativeView = SkiaSharp.Views.UWP.SKSwapChainPanel;

namespace Physics.Shared.UI.Rendering.Skia
{
	public class NonUiSkiaCanvas : SKNativeView, ISkiaCanvas
	{
		public NonUiSkiaCanvas()
		{
			EnableRenderLoop = true;
			DrawInBackground = true;
			PaintSurface += OnNativeControlInitialized;
		}

		private void OnNativeControlInitialized(object sender, SkiaSharp.Views.UWP.SKPaintGLSurfaceEventArgs e)
		{
			PaintSurface -= OnNativeControlInitialized;
			Initialized?.Invoke(this, e.Surface);
			this.PaintSurface += OnRendering;
		}

		private void OnRendering(object sender, SkiaSharp.Views.UWP.SKPaintGLSurfaceEventArgs e)
		{
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

		public event SkiaEventHandler<SKSurface> Initialized;
		public event SkiaEventHandler<EventArgs> Update;
		public event SkiaEventHandler<SKSurface> Draw;
	}
}
