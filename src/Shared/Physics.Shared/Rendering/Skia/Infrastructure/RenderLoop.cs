using System;
using Windows.UI.Xaml.Media;

namespace Physics.Shared.UI.Rendering.Skia.Infrastructure
{
    public sealed class RenderLoop : IRenderLoop
    {
        private bool _isEnabled;
        private Action? _action;

        public void Start(Action action)
        {
            _action = action;

            if (!_isEnabled)
            {
                CompositionTarget.Rendering += OnRendering;
            }

            _isEnabled = true;
        }

        public void Stop()
        {
            if (_isEnabled)
            {
                CompositionTarget.Rendering -= OnRendering;
                _isEnabled = false;
                _action = null;
            }
        }

        private void OnRendering(object sender, object e)
        {
            if (_isEnabled)
            {
                _action?.Invoke();
            }
        }
    }
}
