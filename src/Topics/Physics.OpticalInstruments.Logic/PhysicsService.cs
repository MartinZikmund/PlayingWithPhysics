using System;
using System.Numerics;
using Physics.Shared.Logic.Geometry;

namespace Physics.OpticalInstruments.Logic
{
	public class PhysicsService
	{
		public ObjectImageInfo CalculateObjectImage(InstrumentType instrumentType, SceneConfiguration scene) =>
			instrumentType switch
			{
				InstrumentType.ConvexMirror => CalculateConvexMirrorImage(scene),
				InstrumentType.ConcaveMirror => CalculateConcaveMirrorImage(scene),
				InstrumentType.ConvexLens => CalculateConvexLensImage(scene),
				InstrumentType.ConcaveLens => CalculateConcaveLensImage(scene),
				_ => throw new InvalidOperationException("Unknown instrument"),
			};
		private ObjectImageInfo CalculateConcaveLensImage(SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);

			// Object is beyond the focal distance - image is real, smaller and flipped
			var imageDistance = 1 / (1 / (scene.FocalDistance) - 1 / (scene.ObjectDistance));
			var imageHeight = -imageDistance / (scene.ObjectDistance) * scene.ObjectHeight;

			objectImageInfo.ImageDistance = imageDistance;
			objectImageInfo.ImageHeight = imageHeight;
			objectImageInfo.ImageType = ImageType.Imaginary;

			return objectImageInfo;
		}

		private ObjectImageInfo CalculateConvexLensImage(SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);

			if (Math.Abs(scene.ObjectDistance - scene.FocalDistance) < 0.001)
			{
				objectImageInfo.ImageType = ImageType.None;
				return objectImageInfo;
			}

			if (scene.ObjectDistance < scene.FocalDistance)
			{
				objectImageInfo.ImageType = ImageType.Imaginary;
			}
			else
			{
				objectImageInfo.ImageType = ImageType.Real;
			}

			// Object is beyond the focal distance - image is real, smaller and flipped
			var imageDistance = 1 / ((1 / scene.FocalDistance) - (1 / scene.ObjectDistance));
			var imageHeight = -imageDistance / scene.ObjectDistance * scene.ObjectHeight;

			objectImageInfo.ImageDistance = imageDistance;
			objectImageInfo.ImageHeight = imageHeight;

			return objectImageInfo;
		}

		private ObjectImageInfo CalculateConcaveMirrorImage(SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);

			if (Math.Abs(scene.ObjectDistance - scene.FocalDistance) < 0.001)
			{
				objectImageInfo.ImageType = ImageType.None;
				return objectImageInfo;
			}

			if (scene.ObjectDistance < scene.FocalDistance)
			{
				objectImageInfo.ImageType = ImageType.Imaginary;
			}
			else
			{
				objectImageInfo.ImageType = ImageType.Real;
			}

			// Object is beyond the focal distance - image is real, smaller and flipped
			var imageDistance = 1 / (1 / (scene.FocalDistance) - 1 / scene.ObjectDistance);
			var imageHeight = -imageDistance / scene.ObjectDistance * scene.ObjectHeight;

			objectImageInfo.ImageDistance = imageDistance;
			objectImageInfo.ImageHeight = imageHeight;
			
			return objectImageInfo;
		}

		private ObjectImageInfo CalculateConvexMirrorImage(SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);

			// Object is beyond the focal distance - image is real, smaller and flipped
			var imageDistance = 1 / (1 / (scene.FocalDistance) - 1 / scene.ObjectDistance);
			var imageHeight = -imageDistance / scene.ObjectDistance * scene.ObjectHeight;

			objectImageInfo.ImageType = ImageType.Imaginary;
			objectImageInfo.ImageDistance = imageDistance;
			objectImageInfo.ImageHeight = imageHeight;

			return objectImageInfo;
		}
	}
}
