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
			ImageSizeType imageSizeType)
		{
			var SUT = new PhysicsService();
			var image = SUT.CalculateObjectImage(InstrumentType.ConvexLens, new SceneConfiguration()
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
			Assert.Equal(imageDistance, image.ImageDistance, 2);
			Assert.Equal(imageHeight, image.ImageHeight, 2);
			Assert.Equal(flipped, image.ImageOrientation);
			Assert.Equal(imageSizeType, image.SizeType);
		}
	}
}
