using Microsoft.Graphics.Canvas.UI.Xaml;
using Physics.InclinedPlane.Services;

namespace Physics.HomogenousParticle.Rendering
{
    public interface IVariantRenderer
    {
        void Update(ICanvasAnimatedControl sender);

        void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args);
    }
}
