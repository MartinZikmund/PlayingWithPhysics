using Physics.HomogenousParticle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Physics.HomogenousParticle.Converters
{
    public class BoolToVelocityVariantConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            VelocityVariant variant = Enum.Parse<VelocityVariant>(parameter.ToString(), true);
            return variant == (VelocityVariant)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool isChecked = (bool)value;
            if (isChecked)
            {
                var res = Enum.Parse<VelocityVariant>(parameter.ToString(), true);
                return res;
            }

            return 0;
        }
    }
}
