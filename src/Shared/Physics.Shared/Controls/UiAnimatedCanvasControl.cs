using DynamicData.Annotations;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Physics.Shared.Controls
{
    public class UiAnimatedCanvasControl : ContentControl
    {
        private CanvasSwapChain _swapChain;
        private CanvasSwapChainPanel _swapChainPanel;
        private CanvasDrawingSession _drawingSession;
        private DateTimeOffset _simulationStart;
        private DateTimeOffset _lastUpdateTime;
        private long _updateCount = 0;

        public UiAnimatedCanvasControl()
        {
            this.Loaded += Initialize;
        }

        public ICanvasResourceCreator ResourceCreator => _swapChain;

        public void ResetTime()
        {
            _updateCount = 0;
            _simulationStart = DateTimeOffset.UtcNow;
            _lastUpdateTime = _simulationStart;
        }

        private void Initialize(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (DesignMode.DesignMode2Enabled) return;
            // Get display information.
            GetDisplayInformation(out Size rawSize, out float dpi, out double dpiScale);

            // Create swap chain and hosting panel.
            var swapChain = new CanvasSwapChain(CanvasDevice.GetSharedDevice(), (float)rawSize.Width, (float)rawSize.Height, dpi);
            var swapChainPanel = new CanvasSwapChainPanel();

            _swapChain = swapChain;
            _swapChainPanel = swapChainPanel;
            _swapChainPanel.SwapChain = swapChain;
            // Create drawing session for swap chain.
            _drawingSession = swapChain.CreateDrawingSession(Colors.Transparent);
            // Make sure rendering is done in raw pixels.
            _drawingSession.Units = CanvasUnits.Pixels;

            // Set created swap chain panel as content.
            Content = swapChainPanel;

            _simulationStart = DateTimeOffset.UtcNow;
            _lastUpdateTime = _simulationStart;

            // Register for Rendering event - update processing.
            CompositionTarget.Rendering += OnProcessUpdate;
            // Register for SizeChanged event - swap chain buffer resize.
            SizeChanged += OnSizeChanged;
        }

        private void OnProcessUpdate(object sender, object e)
        {
            var currentTime = DateTimeOffset.UtcNow;
            var elapsed = currentTime - _lastUpdateTime;
            _lastUpdateTime = currentTime;
            Update?.Invoke(this, new CanvasAnimatedUpdateEventArgs(
                new CanvasTimingInformation()
                {
                    ElapsedTime = elapsed,
                    IsRunningSlowly = false,
                    TotalTime = currentTime - _simulationStart,
                    UpdateCount = ++_updateCount
                }));
            Draw?.Invoke(this, new CanvasDrawEventArgs(_drawingSession));
            _drawingSession.Flush();
            _swapChain.Present();
        }

        public event TypedEventHandler<UiAnimatedCanvasControl, CanvasAnimatedUpdateEventArgs> Update;

        public event TypedEventHandler<UiAnimatedCanvasControl, CanvasDrawEventArgs> Draw;

        private void OnSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            // Get display information.
            GetDisplayInformation(out Size rawSize, out float dpi, out _);

            // Dispose old drawing session before resizing buffer otherwise an exception would be thrown.
            _drawingSession.Dispose();
            // Resize swap chain buffers.
            _swapChain.ResizeBuffers((float)rawSize.Width, (float)rawSize.Height, dpi);
            // Create new drawing session.
            _drawingSession = _swapChain.CreateDrawingSession(Colors.Transparent);
            // Make sure rendering is done in raw pixels.
            _drawingSession.Units = CanvasUnits.Pixels;
        }

        private void GetDisplayInformation(out Windows.Foundation.Size rawSize, out float dpi, out double dpiScale)
        {
            var displayInformation = DisplayInformation.GetForCurrentView();

            dpiScale = displayInformation.RawPixelsPerViewPixel;
            dpi = displayInformation.LogicalDpi;
            rawSize = new Size(ActualWidth * dpiScale, ActualHeight * dpiScale);
        }
    }
}
