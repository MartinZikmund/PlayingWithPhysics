﻿using Physics.Shared.UI.ViewModels;
using Physics.HomogenousParticle.ValuesTable;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.HomogenousMovement.ViewModels
{
    public class PrallelVariantValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
    {

        //public ValuesTableDialogViewModel(ITableService<TableRow> tableService) 
        //    : base(tableService)
        //{
        //    _type = movementType;
        //}

        //internal void Reset(TableService tableService, MovementType type)
        //{
        //    _tableService = tableService;
        //    _type = type;
        //    UpdateTable();
        //    OnPropertyChanged(nameof(ButtonVisibility));            
        //}

        //public override void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs)
        //{
        //    if (eventArgs.Column.Header.ToString() == "Time")
        //    {
        //        eventArgs.Column.Header = "t (s)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "X")
        //    {
        //        eventArgs.Column.Header = "x (m)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "Y")
        //    {
        //        eventArgs.Column.Header = "y (m)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "VX")
        //    {
        //        eventArgs.Column.Header = "vx (m/s)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "VY")
        //    {
        //        eventArgs.Column.Header = "vy (m/s)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "V")
        //    {
        //        eventArgs.Column.Header = "v (m/s)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "EP")
        //    {
        //        eventArgs.Column.Header = "Ep (J)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "EK")
        //    {
        //        eventArgs.Column.Header = "Ek (J)";
        //    }

        //    if (eventArgs.Column.Header.ToString() == "EPEK")
        //    {
        //        eventArgs.Column.Header = "Ep + Ek (J)";
        //    }
        //}

        //public void CopyToClipboard()
        //{
        //    var clipboardContents = new StringBuilder();
        //    foreach (var data in Values)
        //    {
        //        clipboardContents.AppendLine(data.ToTabString());
        //    }

        //    var dataPackage = new DataPackage();
        //    dataPackage.SetText(clipboardContents.ToString());
        //    Clipboard.SetContent(dataPackage);
        //}

        //public Visibility ButtonVisibility => (_type == MovementType.FreeFall || _type == MovementType.VerticalMotion)
        //    ? Visibility.Visible
        //    : Visibility.Collapsed;
        public PrallelVariantValuesTableDialogViewModel(ITableService<TableRow> tableService) : base(tableService)
        {
        }
    }
}
