using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.Shared.UI.Helpers;
using Physics.WaveInterference.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Physics.WaveInterference.UserControls
{
	public sealed partial class EditWaveUserControl : UserControl
	{
		public EditWaveUserControl()
		{
			this.InitializeComponent();
			SetupNumberBoxFormattings();
		}

		public EditWaveViewModel Wave
		{
			get => (EditWaveViewModel)GetValue(WaveProperty);
			set => SetValue(WaveProperty, value);
		}

		public static readonly DependencyProperty WaveProperty =
			DependencyProperty.Register(nameof(Wave), typeof(EditWaveViewModel), typeof(EditWaveUserControl), new PropertyMetadata(null));

		private void SetupNumberBoxFormattings()
		{
			AmplitudeNumberBox.SetupFormatting(increment: 0.1, fractionDigits: 1, smallChange: 0.1);
			FrequencyNumberBox.SetupFormatting(fractionDigits: 2, smallChange: 0.01, increment: 0.01);
			WaveLengthNumberBox.SetupFormatting(increment: 0.001, fractionDigits: 3, smallChange: 0.001, largeChange: 0.005);
		}
	}
}
