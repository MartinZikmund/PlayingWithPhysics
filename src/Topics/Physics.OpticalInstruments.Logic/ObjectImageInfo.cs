using Physics.Shared.Logic.Geometry;

namespace Physics.OpticalInstruments.Logic
{
	public class ObjectImageInfo
    {
        public bool IsReal { get; set; }

		public bool IsSmaller { get; set; }

		public bool IsFlipped { get; set; }

		public float ImageDistance { get; set; }

		public float ImageHeight { get; set; }
    }
}
