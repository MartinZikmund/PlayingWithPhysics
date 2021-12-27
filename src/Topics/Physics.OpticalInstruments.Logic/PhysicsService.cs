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
				InstrumentType.ConvexMirror => CalculateConvexMirrorImage(instrumentType, scene),
				InstrumentType.ConcaveMirror => CalculateConcaveMirrorImage(instrumentType, scene),
				InstrumentType.ConvexLens => CalculateConvexLensImage(instrumentType, scene),
				InstrumentType.ConcaveLens => CalculateConcaveLensImage(instrumentType, scene),
				_ => throw new InvalidOperationException("Unknown instrument"),
			};
		private ObjectImageInfo CalculateConcaveLensImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);

			// Object is beyond the focal distance - image is real, smaller and flipped
			var imageDistance = 1 / (1 / (scene.FocalDistance) - 1 / (scene.ObjectDistance));
			var imageHeight = imageDistance / (scene.ObjectDistance) * scene.ObjectHeight;

			objectImageInfo.ImageDistance = imageDistance;
			objectImageInfo.ImageHeight = imageHeight;

			return objectImageInfo;
		}

		private ObjectImageInfo CalculateConvexLensImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);

			if (Math.Abs(scene.ObjectDistance - scene.FocalDistance) < 0.01)
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

		private ObjectImageInfo CalculateConcaveMirrorImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);
			var radius = scene.FocalDistance * 2;
			if (Math.Abs(scene.ObjectDistance) > scene.FocalDistance)
			{
				// Object is beyond the focal distance - image is real, smaller and flipped
				var imageDistance = 1 / (1 / (-scene.FocalDistance) - 1 / scene.ObjectDistance);
				var imageHeight = -imageDistance / scene.ObjectDistance * scene.ObjectHeight;

				objectImageInfo.ImageDistance = imageDistance;
				objectImageInfo.ImageHeight = imageHeight;
			}
			else if (Math.Abs(scene.ObjectDistance) < scene.FocalDistance)
			{
				// Object is closer to mirror than focal distance - image is imaginary, bigger, and upright
				var imageDistance = 1 / (1 / (-scene.FocalDistance) - 1 / scene.ObjectDistance);
				var imageHeight = -imageDistance / scene.ObjectDistance * scene.ObjectHeight;

				objectImageInfo.ImageDistance = imageDistance;
				objectImageInfo.ImageHeight = imageHeight;
			}

			return objectImageInfo;
		}

		private ObjectImageInfo CalculateConvexMirrorImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo(scene.ObjectHeight);
			var radius = scene.FocalDistance * 2;

			// Object is beyond the focal distance - image is real, smaller and flipped
			var imageDistance = 1 / (1 / (scene.FocalDistance) - 1 / scene.ObjectDistance);
			var imageHeight = -imageDistance / scene.ObjectDistance * scene.ObjectHeight;

			objectImageInfo.ImageDistance = imageDistance;
			objectImageInfo.ImageHeight = imageHeight;

			return objectImageInfo;
		}
	}
}
