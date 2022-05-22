using System.Drawing;
using System.Linq;

namespace Physics.HuygensPrinciple.Logic
{
	public class BitmapScenePreset : ScenePreset
	{
		public BitmapScenePreset(HuygensField baseBitmap, string name, PointF[] significantPoints = null) : base(name, significantPoints)
		{
			BaseBitmap = baseBitmap;
		}

		public HuygensField BaseBitmap { get; set; }

		public override ScenePreset Clone()
		{
			var preset = new BitmapScenePreset(BaseBitmap.Clone(), Name, SignificantPoints.ToArray());
			preset.BaseBitmap = BaseBitmap.Clone();
			preset._shapes.AddRange(_shapes);
			return preset;
		}
	}
}
