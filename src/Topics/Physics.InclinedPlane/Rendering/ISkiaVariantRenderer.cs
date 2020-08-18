using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.Rendering
{
    public interface ISkiaVariantRenderer
    {
        void Update(SkiaCanvas sender);

        void Draw(SkiaCanvas sender, SKSurface args);
    }
}
