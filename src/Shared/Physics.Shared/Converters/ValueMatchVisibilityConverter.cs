using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Physics.Shared.UI.Converters
{
	public class ValueMatchVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var areEqual = parameter.Equals(value?.ToString());
			return areEqual ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
	}
}
