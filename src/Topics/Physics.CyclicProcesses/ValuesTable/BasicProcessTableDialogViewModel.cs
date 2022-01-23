using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;

namespace Physics.CyclicProcesses.ValuesTable
{
	public class BasicProcessTableDialogViewModel : ValuesTableDialogViewModelBase<BasicProcessTableRow>
	{
		public BasicProcessTableDialogViewModel(ITableService<BasicProcessTableRow> tableService) : base(tableService)
		{
		}
	}
}
