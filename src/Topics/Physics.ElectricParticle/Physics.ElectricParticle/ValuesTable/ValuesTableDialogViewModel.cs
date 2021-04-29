using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;
using System;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Physics.Shared.Extensions;

namespace Physics.ElectricParticle.ValuesTable
{
    public class ValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
    {
        public ValuesTableDialogViewModel(ITableService<TableRow> tableService)
            : base(tableService)
        {
			if (tableService is TableService table)
			{
				table.ViewModel = this;
			}
        }

		public int Steps { get; set; } = 50;

		public void OnStepsChanged() => UpdateTable();

		internal void Reset(TableService tableService)
        {
            _tableService = tableService;
			//_type = type;
			UpdateTable();
        }

        public override void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("Time"))
            {
                eventArgs.Column.Header = "t (s)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("X"))
            {
                eventArgs.Column.Header = "x (m)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("Y"))
            {
                eventArgs.Column.Header = "y (m)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("Velocity"))
            {
                eventArgs.Column.Header = "v (m/s)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("VelocityX"))
            {
                eventArgs.Column.Header = "vx (m/s)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("VelocityY"))
            {
                eventArgs.Column.Header = "vy (m/s)";
            }

			if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("Acceleration"))
			{
				eventArgs.Column.Header = "a (m/s^2)";
			}

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("EP"))
            {
                eventArgs.Column.Header = "Ep (J)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("EK"))
            {
                eventArgs.Column.Header = "Ek (J)";
            }

            if (eventArgs.Column.Header.ToString().EqualsInvariantIgnoreCase("E"))
            {
                eventArgs.Column.Header = "E (J)";
            }
        }

        public void CopyToClipboard()
        {
            var clipboardContents = new StringBuilder();
            foreach (var data in Values)
            {
                clipboardContents.AppendLine(data.ToTabString());
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(clipboardContents.ToString());
            Clipboard.SetContent(dataPackage);
        }
    }
}
