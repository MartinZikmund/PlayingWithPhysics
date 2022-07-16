using System;
using Windows.UI.Xaml.Data;
using Physics.Shared.UI.Localization;

namespace Physics.Shared.UI.Converters
{
    public class EnumLocalizationConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
			{
				return "";
			}
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
