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
			return new ObjectImageInfo()
			{
				IsReal = false
			};
		}

		private ObjectImageInfo CalculateConvexLensImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			return new ObjectImageInfo()
			{
				IsReal = false
			};
		}

		private ObjectImageInfo CalculateConcaveMirrorImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			var objectImageInfo = new ObjectImageInfo();
			var radius = scene.FocalDistance * 2;
			if (Math.Abs(scene.ObjectDistance) > scene.FocalDistance)
			{
				// Object is beyond the radius distance - image is real, smaller and flipped
				objectImageInfo.IsReal = true;
				objectImageInfo.IsSmaller = true;
				objectImageInfo.IsFlipped = true;

				var imageDistance = 1 / (1 / (-scene.FocalDistance) - 1 / scene.ObjectDistance);
				var imageHeight = - imageDistance / scene.ObjectDistance * scene.ObjectHeight;

				objectImageInfo.ImageDistance = imageDistance;
				objectImageInfo.ImageHeight = imageHeight;
			}

			return objectImageInfo;
		}

		private ObjectImageInfo CalculateConvexMirrorImage(InstrumentType instrumentType, SceneConfiguration scene)
		{
			return new ObjectImageInfo()
			{
				IsReal = false
			};
		}
	}
}
