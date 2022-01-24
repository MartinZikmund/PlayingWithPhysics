using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using SkiaSharp;
using Windows.UI;

namespace Physics.Shared.UI.Extensions
{
    public static class ColorExtensions
    {
        public static double GetPerceivedLuminance(this Color color)
        {
            return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
        }

		public static SKColor ToSKColor(this string hexString)
		{
			var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(hexString);
			return new SKColor(color.R, color.G, color.B, color.A);
		}
    }
}
