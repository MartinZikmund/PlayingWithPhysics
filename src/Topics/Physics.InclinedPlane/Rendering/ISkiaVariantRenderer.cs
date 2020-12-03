using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Rendering
{
    public interface ISkiaVariantRenderer : IDisposable
    {
        void Update(ISkiaCanvas sender);

        void Draw(ISkiaCanvas sender, SKSurface args);
    }
}
