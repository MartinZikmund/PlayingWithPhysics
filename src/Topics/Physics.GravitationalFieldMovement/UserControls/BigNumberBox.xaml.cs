using System;
using Microsoft.UI.Xaml.Controls;
using Physics.Shared.Mathematics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.GravitationalFieldMovement.UserControls
{
	public sealed partial class BigNumberBox : UserControl
	{
		public BigNumberBox() => InitializeComponent();

		public NumberBox MantisaBox => MantisaNumberBox;

		public NumberBox ExponentBox => ExponentNumberBox;

		private double Mantisa
		{
			get => _mantisa;
			set
			{
				if (double.IsNaN(value) || _mantisa == value)
				{
					return;
				}

				_mantisa = value;
				Value = new BigNumber(value, Value.Exponent);
			}
		}

		private int Exponent
		{
			get => _exponent;
			set
			{
				if (double.IsNaN(value) || _exponent == value)
				{
					return;
				}

				_exponent = value;
				Value = new BigNumber(Value.Mantisa, value);
			}
		}

		public BigNumber Value
		{
			get => (BigNumber)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(BigNumber), typeof(BigNumberBox), new PropertyMetadata(0, OnValueChanged));
		private double _mantisa;
		private int _exponent;

		private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			var instance = (BigNumberBox)sender;
			if (args.NewValue is BigNumber bigNumber)
			{
				instance.Mantisa = bigNumber.Mantisa;
				instance.Exponent = bigNumber.Exponent;
			}
		}
	}
}
