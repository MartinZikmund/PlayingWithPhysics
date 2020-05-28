using Physics.Shared.UI.ViewModels;
using Physics.HomogenousParticle.ValuesTable;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.HomogenousMovement.ViewModels
{
    public class PerpendicularTableDialogViewModel : ValuesTableDialogViewModelBase<PerpendicularTableRow>
    {

        public PerpendicularTableDialogViewModel(ITableService<PerpendicularTableRow> tableService)
            : base(tableService)
        {            
        }

        internal void Reset(ITableService<PerpendicularTableRow> tableService)
        {
            _tableService = tableService;            
            UpdateTable();
        }

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
    }
}
