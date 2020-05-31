using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Rendering
{
    public interface IRenderer
    {
        void Update(ICanvasAnimatedControl sender);

        void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args);
    }
}
