using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Physics.HomogenousMovement.Converters
{
    public class HexColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is Windows.UI.Color)) return null;

            Windows.UI.Color c = (Windows.UI.Color)value;
            string hex = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
            return hex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string)) return null;

            Windows.UI.Color c = new Windows.UI.Color();
            string t = (string)value;

            try
            {
                c.A = (byte)255;
                c.R = (byte)int.Parse(t.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                c.G = (byte)int.Parse(t.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                c.B = (byte)int.Parse(t.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                throw (new ArgumentException("Invalid color. Must be something like #AABBCC"));
            }

            return c;
        }
    }
}
