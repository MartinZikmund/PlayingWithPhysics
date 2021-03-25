using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Physics.Shared.UI.ViewModels
{
    public class ValuesTableDialogViewModelBase<TValuesTableRow> : INotifyPropertyChanged
        where TValuesTableRow : IValuesTableRow
    {
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
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
