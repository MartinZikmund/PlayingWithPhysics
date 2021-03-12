using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Rendering.Skia;
using SkiaSharp;

namespace Physics.RadiationHalflife.Rendering
{
	public class RadiationHalflifeController : SkiaCanvasController
	{
		public override void Draw(ISkiaCanvas sender, SKSurface args) => Debug.WriteLine("Test");
		public override void Update(ISkiaCanvas sender) => Debug.WriteLine("Test");
		public RadiationHalflifeController(ISkiaCanvas canvasAnimatedControl) : base(canvasAnimatedControl)
		{
		}
	}
}
