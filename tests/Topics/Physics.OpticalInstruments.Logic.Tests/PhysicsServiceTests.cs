using Xunit;

namespace Physics.OpticalInstruments.Logic.Tests
{
	public class PhysicsServiceTests
	{
		[InlineData(0.075f, 0.3f, 1f, 0.1f, -0.333f, ImageType.Real, ImageOrientationType.Flipped, ImageSizeType.Smaller)]
		[InlineData(1f, 1f, 1f, 0f, 0f, ImageType.None, default(ImageOrientationType), default(ImageSizeType))]
		[InlineData(1f, 2f, 1f, 2f, -1f, ImageType.Real, ImageOrientationType.Flipped, ImageSizeType.Same)]
		[InlineData(1f, 1.5f, 1f, 3f, -2f, ImageType.Real, ImageOrientationType.Flipped, ImageSizeType.Larger)]
		[InlineData(1f, 0.5f, 1f, -1f, 2f, ImageType.Imaginary, ImageOrientationType.Straight, ImageSizeType.Larger)]
		[Theory]
		public void When_ConvexLens(
			float focalDistance,
			float objectDistance,
			float objectHeight,
			float imageDistance,
			float imageHeight,
			ImageType imageType,
			ImageOrientationType flipped,
			ImageSizeType imageSizeType) =>
			VerifyImageProperties(
				InstrumentType.ConvexLens,
				focalDistance,
				objectDistance,
				objectHeight,
				imageDistance,
				imageHeight,
				imageType,
				flipped,
				imageSizeType);

		[InlineData(-0.167f, 2f, 0.4f, -0.154f, 0.031f, ImageType.Imaginary, ImageOrientationType.Straight, ImageSizeType.Smaller)]
		[InlineData(-0.2f, 0.15f, 0.01f, -0.0857f, 4/700.0f, ImageType.Imaginary, ImageOrientationType.Straight, ImageSizeType.Smaller)]
		[Theory]
		public void When_ConcaveLens(
			float focalDistance,
			float objectDistance,
			float objectHeight,
			float imageDistance,
			float imageHeight,
			ImageType imageType,
			ImageOrientationType flipped,
			ImageSizeType imageSizeType) =>
			VerifyImageProperties(
				InstrumentType.ConcaveLens,
				focalDistance,
				objectDistance,
				objectHeight,
				imageDistance,
				imageHeight,
				imageType,
				flipped,
				imageSizeType);


		[InlineData(0.25f, 0.8f, 0.03f, 0.364f, -0.0136f, ImageType.Real, ImageOrientationType.Flipped, ImageSizeType.Smaller)]
		[Theory]
		public void When_ConcaveMirror(
			float focalDistance,
			float objectDistance,
			float objectHeight,
			float imageDistance,
			float imageHeight,
			ImageType imageType,
			ImageOrientationType flipped,
			ImageSizeType imageSizeType) =>
			VerifyImageProperties(
				InstrumentType.ConcaveMirror,
				focalDistance,
				objectDistance,
				objectHeight,
				imageDistance,
				imageHeight,
				imageType,
				flipped,
				imageSizeType);

		[InlineData(-0.075f, 5f, 1.8f, -0.074f, 0.027f, ImageType.Imaginary, ImageOrientationType.Straight, ImageSizeType.Smaller)]
		[Theory]
		public void When_ConvexMirror(
			float focalDistance,
			float objectDistance,
			float objectHeight,
			float imageDistance,
			float imageHeight,
			ImageType imageType,
			ImageOrientationType flipped,
			ImageSizeType imageSizeType) =>
			VerifyImageProperties(
				InstrumentType.ConvexMirror,
				focalDistance,
				objectDistance,
				objectHeight,
				imageDistance,
				imageHeight,
				imageType,
				flipped,
				imageSizeType);

		private void VerifyImageProperties(
			InstrumentType instrumentType,
			float focalDistance,
			float objectDistance,
			float objectHeight,
			float imageDistance,
			float imageHeight,
			ImageType imageType,
			ImageOrientationType flipped,
			ImageSizeType imageSizeType)
		{
			var SUT = new PhysicsService();
			var image = SUT.CalculateObjectImage(instrumentType, new SceneConfiguration()
			{
				FocalDistance = focalDistance,
				ObjectDistance = objectDistance,
				ObjectHeight = objectHeight
			});
			Assert.Equal(imageType, image.ImageType);
			if (imageType == ImageType.None)
			{
				return;
			}
			Assert.Equal(imageDistance, image.ImageDistance, 3f);
			Assert.Equal(imageHeight, image.ImageHeight, 3f);
			Assert.Equal(flipped, image.ImageOrientation);
			Assert.Equal(imageSizeType, image.SizeType);
		}
	}
}
