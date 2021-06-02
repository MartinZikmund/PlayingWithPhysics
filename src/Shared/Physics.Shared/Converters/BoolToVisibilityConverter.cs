using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Physics.Shared.UI.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public bool Inverted { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool boolValue)
			{
				if (Inverted)
				{
					boolValue = !boolValue;
				}

				return boolValue ? Visibility.Visible : Visibility.Collapsed;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is Visibility visibilityValue)
			{
				if (Inverted)
				{
					visibilityValue = visibilityValue == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
				}

				return visibilityValue == Visibility.Visible;
			}

			return false;
		}
	}
}
