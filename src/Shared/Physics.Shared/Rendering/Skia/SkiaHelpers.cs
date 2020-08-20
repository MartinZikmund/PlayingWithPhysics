using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Rendering.Skia
{
    public static class SkiaHelpers
    {
        public static SKSize StretchUniformToFill(this SKSize originalSize, SKSize targetSize)
        {
            var widthScale = targetSize.Width / originalSize.Width;
            var heightScale = targetSize.Height / originalSize.Height;
            var finalScale = Math.Max(widthScale, heightScale);
            return new SKSize(finalScale * originalSize.Width, finalScale * originalSize.Height);
        }

        public static SKBitmap RotateBitmap(SKBitmap bitmap, double angle)
        {
            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            var rotatedBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

            using (var surface = new SKCanvas(rotatedBitmap))
            {
                surface.Clear();
                surface.Translate(rotatedWidth / 2, rotatedHeight / 2);
                surface.RotateDegrees((float)angle);
                surface.Translate(-originalWidth / 2, -originalHeight / 2);
                surface.DrawBitmap(bitmap, new SKPoint());
            }
            return rotatedBitmap;
        }

        public static SKPoint RotatePoint(SKPoint point, int centerX, int centerY, float rad)
        {
            var sin = (float)Math.Sin(rad);
            var cos = (float)Math.Cos(rad);
            var translatedX = point.X - centerX;
            var translatedY = point.Y - centerY;
            var rotatedX = translatedX * cos - translatedY * sin;
            var rotatedY = translatedX * sin + translatedY * cos;
            return new SKPoint(rotatedX + centerX, rotatedY + centerY);
        }
        
        private static float ToRadians(float degrees)
        {
            return (float)(Math.PI * degrees / 180.0);
        }
    }
}