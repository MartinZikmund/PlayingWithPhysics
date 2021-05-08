using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RadiationHalflife.Rendering
{
	public class BeamActivityRenderer : IVariantRenderer
	{
		public void Draw(ISkiaCanvas sender, SKSurface args) => throw new NotImplementedException();
		public void Update(ISkiaCanvas sender) => throw new NotImplementedException();
		public void Dispose() { }
		public void StartSimulation() { }

		public void SetAnimation(PhysicsService physicsService)
		{
			PhysicsService = (BeamActivityPhysicsService)physicsService;
		}

		public RadiationHalflifeController _controller;

		public BeamActivityPhysicsService PhysicsService { get; set; }

		public BeamActivityRenderer(RadiationHalflifeController controller)
		{
			_controller = controller;
		}
	}
}
