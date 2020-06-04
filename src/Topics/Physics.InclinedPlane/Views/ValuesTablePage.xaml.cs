using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.InclinedPlane.ValuesTable;
using Physics.Shared.UI.Infrastructure.Topics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Physics.InclinedPlane.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ValuesTablePage : Page
    {
        private DifficultyOption _difficulty;
        public ValuesTablePage()
        {
            this.InitializeComponent();
        }

        public void Initialize(ValuesTableDialogViewModel viewModel)
        {
            Model = viewModel;
            DataContext = Model;
            SetupFormatting();
        }

        private void SetupFormatting()
        {
            var rounder = new IncrementNumberRounder();
            rounder.Increment = 0.1;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            var formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 1;
            formatter.NumberRounder = rounder;
            TimeIntervalNumberBox.NumberFormatter = formatter;
        }

        public ValuesTableDialogViewModel Model { get; set; }

        //TODO: Translate button content
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //SwitchColumnsVisibility();
        }

        private void SwitchColumnsVisibility()
        {
            var xColumn = ValuesTable.Columns[1];
            var v0Column = ValuesTable.Columns[4];
            if (xColumn != null && v0Column != null)
            {
                Visibility newVisibility = Visibility.Visible;
                if (xColumn.Visibility == Visibility.Collapsed)
                {
                    newVisibility = Visibility.Visible;
                }
                else
                {
                    newVisibility = Visibility.Collapsed;
                }
                xColumn.Visibility = newVisibility;
                v0Column.Visibility = newVisibility;
            }
        }

        private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) =>
            Model?.AdjustColumnHeaders(e);
    }
}
