using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.VoiceCommands;
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

namespace Physics.HomogenousMovement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ValuesTablePage : Page
    {
        private MovementType _type;
        public ValuesTablePage()
        {
            this.InitializeComponent();
            Model = new ValuesTableDialogViewModel();
            DataContext = Model;
        }

        public void Initialize(IPhysicsService service, MovementType type)
        {
            SetupFromatting();
            _type = type;
            Model.Initalize(service, type);
        }

        private void SetupFromatting()
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
            SwitchColumnsVisibility();
        }

        private void SwitchColumnsVisibility()
        {
            var xColumn = ValuesTable.Columns.First(column => "X (m)".Equals(column.Header));
            var v0Column = ValuesTable.Columns.First(column => "Vx (m/s)".Equals(column.Header));
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

        private void ValuesTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Time")
            {
                e.Column.Header = "T (s)";
            }

            if (e.Column.Header.ToString() == "X")
            {
                e.Column.Header = "X (m)";
            }

            if (e.Column.Header.ToString() == "Y")
            {
                e.Column.Header = "Y (m)";
            }

            if (e.Column.Header.ToString() == "VX")
            {
                e.Column.Header = "Vx (m/s)";
            }

            if (e.Column.Header.ToString() == "VY")
            {
                e.Column.Header = "Vy (m/s)";
            }

            if (e.Column.Header.ToString() == "V")
            {
                e.Column.Header = "V (m/s)";
            }

            if (e.Column.Header.ToString() == "EP")
            {
                e.Column.Header = "Ep (N)";
            }

            if (e.Column.Header.ToString() == "EK")
            {
                e.Column.Header = "Ek (N)";
            }

            if (e.Column.Header.ToString() == "EPEK")
            {
                e.Column.Header = "EpEk (N)";
            }
        }
    }
}
