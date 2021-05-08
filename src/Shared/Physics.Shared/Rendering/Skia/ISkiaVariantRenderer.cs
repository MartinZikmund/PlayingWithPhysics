using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;

namespace Physics.Shared.UI.Rendering.Skia
{
	public interface ISkiaVariantRenderer : IDisposable
    {
        void Update(ISkiaCanvas sender);

        void Draw(ISkiaCanvas sender, SKSurface args);
    }
}
