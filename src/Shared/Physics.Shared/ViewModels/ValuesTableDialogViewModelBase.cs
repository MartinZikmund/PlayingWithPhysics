using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.ApplicationModel.DataTransfer;

namespace Physics.Shared.UI.ViewModels
{
    public class ValuesTableDialogViewModelBase<TValuesTableRow> : INotifyPropertyChanged
        where TValuesTableRow : IValuesTableRow
    {
        private const char TabSeparator = '\t';
        protected ITableService<TValuesTableRow> _tableService;

        public ValuesTableDialogViewModelBase(ITableService<TValuesTableRow> tableService)
        {
            if (tableService is null)
            {
                throw new ArgumentNullException(nameof(tableService));
            }
            _tableService = tableService;

            UpdateTable();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public float TimeInterval { get; set; } = 0.1f;

        public ObservableCollection<TValuesTableRow> Values { get; private set; }

        public void OnTimeIntervalChanged() => UpdateTable();

        protected void UpdateTable()
        {
            Values = new ObservableCollection<TValuesTableRow>(_tableService.CalculateTable(TimeInterval));            
        }

        public virtual void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
			var propertyName = eventArgs.PropertyName;
			var rowType = typeof(TValuesTableRow);
			var headerAttributes = rowType.GetProperty(propertyName).GetCustomAttributes(typeof(ValuesTableHeaderAttribute), true);
			if (headerAttributes?.FirstOrDefault() is ValuesTableHeaderAttribute attribute)
			{
				eventArgs.Column.Header = attribute.Header;
			}
		}

		public virtual void CopyToClipboard()
		{
			var clipboardContents = new StringBuilder();

			foreach (var property in typeof(TValuesTableRow).GetProperties())
			{
				var header = property.Name;
				var headerAttributes = property.GetCustomAttributes(typeof(ValuesTableHeaderAttribute), true);
				if (headerAttributes?.FirstOrDefault()  is ValuesTableHeaderAttribute attribute)
				{
					header = attribute.Header;
				}

				if (clipboardContents.Length != 0)
				{
					clipboardContents.Append(TabSeparator);
				}
				clipboardContents.Append(header);
			}

			clipboardContents.AppendLine();

			foreach (var data in Values)
			{
				clipboardContents.AppendLine(data.ToTabString());
			}

			var dataPackage = new DataPackage();
			dataPackage.SetText(clipboardContents.ToString());
			Clipboard.SetContent(dataPackage);
		}

		public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
