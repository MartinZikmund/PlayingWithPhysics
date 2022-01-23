using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Physics.Shared.UI.ViewModels
{
	public interface IValuesTableDialogViewModel
	{
		void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs);

		void CopyToClipboard();
	}
}
