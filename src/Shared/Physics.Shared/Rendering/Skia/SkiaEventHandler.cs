using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Rendering.Skia
{
    public delegate void SkiaEventHandler<TEventArgs>(SkiaCanvas canvs, TEventArgs args);
}
