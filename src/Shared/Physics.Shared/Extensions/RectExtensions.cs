﻿using SkiaSharp;
using Windows.Foundation;

namespace Physics.Shared.UI.Extensions
{
    public static class RectExtensions
    {
        public static float ScaleToFit(this Size source, Size target)
        {
            if (target.IsEmpty) return 0;
            if (source.IsEmpty) return 0;

            var targetRatio = target.Height / target.Width;
            var sourceRatio = source.Height / source.Width;

            if ((targetRatio == sourceRatio) ||
                (sourceRatio > targetRatio))
            {
                return (float)(target.Height / source.Height);
            }
            else
            {
                return (float)(target.Width / source.Width);
            }
        }

        public static float ScaleToFit(this Size source, SKSize target)
        {
            return source.ScaleToFit(new Size(target.Width, target.Height));
        }

        public static Size ReduceBy(this Size size, float padding) =>
            new Size(size.Width - padding * 2, size.Height - padding * 2);

        public static SKSize ReduceBy(this SKSize size, float padding) =>
            new SKSize((float)(size.Width - padding * 2), (float)(size.Height - padding * 2));
    }
}
