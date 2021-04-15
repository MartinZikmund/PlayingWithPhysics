using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.ElectricParticle.ValuesTable;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.ElectricParticle.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ValuesTablePage : Page
    {
        public ValuesTablePage()
        {
            this.InitializeComponent();
        }

		public void Initialize(ValuesTableDialogViewModel viewModel)
		{
			Model = viewModel;
			DataContext = Model;
		}

		public ValuesTableDialogViewModel Model { get; set; }

		private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			Model?.AdjustColumnHeaders(e);
		}
	}
}
