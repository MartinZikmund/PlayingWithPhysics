using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Physics.Shared.Helpers;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Physics.ElectricParticle.Converters
{
	public class VoltageColorConverter : IValueConverter
	{
		public bool PositiveIsBlue { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var doubleValue = System.Convert.ToDouble(value);
			doubleValue = MathHelpers.Clamp(doubleValue, -100, 100);

			if (!PositiveIsBlue)
			{
				doubleValue = -doubleValue;
			}

			if (doubleValue > 0)
			{
				return Color.FromArgb(255, 0, 0, (byte)(doubleValue + 150));
			}
			else if (doubleValue < 0)
			{
				return Color.FromArgb(255, (byte)(Math.Abs(doubleValue) + 150), 0, 0);
			}
			return Colors.Black;
		}
		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
	}
}
