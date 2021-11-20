using Physics.HuygensPrinciple.Logic;
using Physics.Shared.UI.Rendering.Skia;

namespace Physics.HuygensPrinciple.Rendering
{
	public interface IHuygensVariantRenderer : ISkiaVariantRenderer
    {
		void StartSimulation();
    }
}
