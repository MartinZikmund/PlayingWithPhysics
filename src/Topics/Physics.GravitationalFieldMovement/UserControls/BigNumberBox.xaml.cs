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
			get => (double)GetValue(MantisaProperty);
			set => SetValue(MantisaProperty, value);
		}

		public static readonly DependencyProperty MantisaProperty =
			DependencyProperty.Register(nameof(Mantisa), typeof(double), typeof(BigNumberBox), new PropertyMetadata(0.0, OnMantisaChanged));

		private static void OnMantisaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var box = (BigNumberBox)d;
			box.UpdateValue();
		}

		private int Exponent
		{
			get => (int)GetValue(ExponentProperty);
			set => SetValue(ExponentProperty, value);
		}

		public static readonly DependencyProperty ExponentProperty =
			DependencyProperty.Register(nameof(Exponent), typeof(int), typeof(BigNumberBox), new PropertyMetadata(0, OnExponentChanged));

		private static void OnExponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var box = (BigNumberBox)d;
			box.UpdateValue();
		}

		private void UpdateValue()
		{
			Value = new BigNumber(Mantisa, Exponent);
		}

		public BigNumber Value
		{
			get => (BigNumber)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(BigNumber), typeof(BigNumberBox), new PropertyMetadata(default(BigNumber), OnValueChanged));

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
