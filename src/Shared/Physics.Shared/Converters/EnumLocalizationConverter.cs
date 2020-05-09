using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Physics.Shared.Extensions;
using Physics.Shared.UI.Services.Localization;

namespace Physics.Shared.UI.Converters
{
    public class EnumLocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var type = value.GetType();
            var valueName = Enum.GetName(type, value);
            var localizationKey = $"{type.Name}_{valueName}";
            return Localizer.Instance.GetString(localizationKey);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
