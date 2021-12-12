using System;
using Physics.Shared.Logic.Geometry;

namespace Physics.OpticalInstruments.Logic
{
	public class ObjectImageInfo
    {
		private readonly float _objectHeight;

		public ObjectImageInfo(float objectHeight)
		{
			_objectHeight = objectHeight;
		}

		public bool IsReal => ImageDistance < 0;

		public bool IsSmaller => Math.Abs(ImageHeight) > Math.Abs(_objectHeight);

		public bool IsFlipped => _objectHeight * ImageHeight < 0;

		public float ImageDistance { get; set; }

		public float ImageHeight { get; set; }
    }
}
