using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Rendering
{
    public interface ISkiaRenderer
    {
        void Update(ISkiaCanvas control);

        void Draw(ISkiaCanvas control, SKSurface surface);
    }
}
