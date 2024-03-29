﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Physics.Shared.UI.Converters
{
	public class NonEmptyStringVisibilityConverter : IValueConverter
	{
		public bool Invert { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var stringValue = value as string;
			var isVisible = !string.IsNullOrEmpty(stringValue);
			if (Invert)
			{
				isVisible = !isVisible;
			}
			return isVisible ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
