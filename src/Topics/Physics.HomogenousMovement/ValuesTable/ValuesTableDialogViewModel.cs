using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using MvvmCross.ViewModels;
using Physics.HomogenousMovement.PhysicsServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Services.ValuesTable;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace Physics.HomogenousMovement.ViewModels
{
    public class ValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
    {
        private MovementType _type;

        public ValuesTableDialogViewModel(ITableService<TableRow> tableService, MovementType movementType) 
            : base(tableService)
        {
            _type = movementType;
        }

        internal void Reset(TableService tableService, MovementType type)
        {
            _tableService = tableService;
            _type = type;
            UpdateTable();
            OnPropertyChanged(nameof(ButtonVisibility));            
        }

        public Visibility ButtonVisibility => (_type == MovementType.FreeFall || _type == MovementType.VerticalMotion)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}
