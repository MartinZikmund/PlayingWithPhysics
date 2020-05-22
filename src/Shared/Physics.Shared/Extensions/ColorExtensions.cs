using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Physics.Shared.UI.Extensions
{
    public static class ColorExtensions
    {
        public static double GetPerceivedLuminance(this Color color)
        {
            return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
        }
    }
}
