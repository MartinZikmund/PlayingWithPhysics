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
    }
}
