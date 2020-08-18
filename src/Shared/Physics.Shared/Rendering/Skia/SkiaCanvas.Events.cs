using SkiaSharp;
using System;

namespace Physics.Shared.UI.Rendering.Skia
{
    public partial class SkiaCanvas
    {
        public event SkiaEventHandler<SKSurface> Initialized;
        public event SkiaEventHandler<EventArgs> Update;

        public
#if __IOS__
            new
#endif
            event SkiaEventHandler<SKSurface> Draw;
    }
}
