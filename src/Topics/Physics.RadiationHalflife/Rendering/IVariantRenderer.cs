using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Rendering.Skia;

namespace Physics.RadiationHalflife.Rendering
{
	public interface IVariantRenderer : ISkiaVariantRenderer
	{
		void StartSimulation();
		void SetAnimation(PhysicsService physicsService);
	}
}
