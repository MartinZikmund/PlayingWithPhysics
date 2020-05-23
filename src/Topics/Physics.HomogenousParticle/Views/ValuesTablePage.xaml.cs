using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.HomogenousParticle.ViewModels;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.HomogenousParticle.Views
{
    public sealed partial class ValuesTablePage : Page
    {        
        public ValuesTablePage()
        {
            this.InitializeComponent();
        }

        //public void Initialize()
        //{
        //    SetupFormatting();
        //}

        //private void SetupFormatting()
        //{
        //    var rounder = new IncrementNumberRounder();
        //    rounder.Increment = 0.1;
        //    rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

        //    var formatter = new DecimalFormatter();
        //    formatter.IntegerDigits = 1;
        //    formatter.FractionDigits = 1;
        //    formatter.NumberRounder = rounder;
        //    TimeIntervalNumberBox.NumberFormatter = formatter;
        //}

        //public ValuesTableDialogViewModel Model { get; set; }

        ////TODO: Translate button content
        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    SwitchColumnsVisibility();
        //}

        //private void SwitchColumnsVisibility()
        //{
        //    var xColumn = ValuesTable.Columns.First(column => "X (m)".Equals(column.Header));
        //    var v0Column = ValuesTable.Columns.First(column => "Vx (m/s)".Equals(column.Header));
        //    if (xColumn != null && v0Column != null)
        //    {
        //        Visibility newVisibility = Visibility.Visible;
        //        if (xColumn.Visibility == Visibility.Collapsed)
        //        {
        //            newVisibility = Visibility.Visible;
        //        }
        //        else
        //        {
        //            newVisibility = Visibility.Collapsed;
        //        }
        //        xColumn.Visibility = newVisibility;
        //        v0Column.Visibility = newVisibility;
        //    }
        //}

        //private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (e.Column.Header.ToString() == "Time")
        //    {
        //        e.Column.Header = "t (s)";
        //    }

        //    if (e.Column.Header.ToString() == "X")
        //    {
        //        e.Column.Header = "x (m)";
        //    }

        //    if (e.Column.Header.ToString() == "Y")
        //    {
        //        e.Column.Header = "y (m)";
        //    }

        //    if (e.Column.Header.ToString() == "VX")
        //    {
        //        e.Column.Header = "vx (m/s)";
        //    }

        //    if (e.Column.Header.ToString() == "VY")
        //    {
        //        e.Column.Header = "vy (m/s)";
        //    }

        //    if (e.Column.Header.ToString() == "V")
        //    {
        //        e.Column.Header = "v (m/s)";
        //    }

        //    if (e.Column.Header.ToString() == "EP")
        //    {
        //        e.Column.Header = "Ep (J)";
        //    }

        //    if (e.Column.Header.ToString() == "EK")
        //    {
        //        e.Column.Header = "Ek (J)";
        //    }

        //    if (e.Column.Header.ToString() == "EPEK")
        //    {
        //        e.Column.Header = "EpEk (J)";
        //    }
        //}
    }
}
