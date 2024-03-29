﻿using System;
using System.Threading.Tasks;
using SkiaSharp;
using Windows.Foundation;
using Windows.UI.Core;

namespace Physics.Shared.UI.Rendering.Skia
{
	public interface ISkiaCanvas
	{
		event SkiaEventHandler<SKSurface> Initialized;
		event SkiaEventHandler<EventArgs> Update;

#if __IOS__
            new
#endif
		event SkiaEventHandler<SKSurface> Draw;

		CoreDispatcher Dispatcher { get; }

		SKSize ScaledSize { get; }

		SKSize NativeSize { get; }

		double ActualWidth { get; }

		double ActualHeight { get; }

		float ScaleFactor { get; }

		bool ShouldRender { get; set; }

		Task RunOnRenderThreadAsync(Action action);
	}
}
