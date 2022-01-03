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

		public bool ImageExists => ImageType != ImageType.None;

		public ImageType ImageType { get; set; }

		public ImageSizeType SizeType
		{
			get
			{
				var diff = Math.Abs(ImageHeight) - Math.Abs(_objectHeight);

				if (Math.Abs(diff) < 0.00001f)
				{
					return ImageSizeType.Same;
				}
				else if (diff > 0)
				{
					return ImageSizeType.Larger;
				}
				else
				{
					return ImageSizeType.Smaller;
				}
			}
		}

		public ImageOrientationType ImageOrientation => _objectHeight * ImageHeight < 0 ? ImageOrientationType.Flipped : ImageOrientationType.Straight;

		public float ImageDistance { get; set; }

		public string ImageDistanceString => (ImageDistance * 100).ToString("0.0");

		public float ImageHeight { get; set; }

		public string ImageHeightString => (ImageHeight * 100).ToString("0.0");
	}
}
