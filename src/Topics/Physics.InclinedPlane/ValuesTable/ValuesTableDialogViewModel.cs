﻿using Microsoft.Toolkit.Uwp.UI.Controls;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.Shared.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;

namespace Physics.InclinedPlane.ValuesTable
{
    public class ValuesTableDialogViewModel : ValuesTableDialogViewModelBase<TableRow>
    {
        private DifficultyOption _difficulty;

        public ValuesTableDialogViewModel(ITableService<TableRow> tableService, DifficultyOption movementType)
            : base(tableService)
        {
            _difficulty = movementType;
        }

        internal void Reset(TableService tableService, DifficultyOption difficulty)
        {
            _tableService = tableService;
            _difficulty = difficulty;
            UpdateTable();
            OnPropertyChanged(nameof(ButtonVisibility));
        }

        public override void AdjustColumnHeaders(DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
            if (eventArgs.Column.Header.ToString() == "Time")
            {
                eventArgs.Column.Header = "t (s)";
            }

            if (eventArgs.Column.Header.ToString() == "X")
            {
                eventArgs.Column.Header = "x (m)";
            }

            if (eventArgs.Column.Header.ToString() == "Y")
            {
                eventArgs.Column.Header = "y (m)";
            }

            if (eventArgs.Column.Header.ToString() == "VX")
            {
                eventArgs.Column.Header = "vx (m/s)";
            }

            if (eventArgs.Column.Header.ToString() == "VY")
            {
                eventArgs.Column.Header = "vy (m/s)";
            }

            if (eventArgs.Column.Header.ToString() == "V")
            {
                eventArgs.Column.Header = "v (m/s)";
            }

            if (eventArgs.Column.Header.ToString() == "EP")
            {
                eventArgs.Column.Header = "Ep (J)";
            }

            if (eventArgs.Column.Header.ToString() == "EK")
            {
                eventArgs.Column.Header = "Ek (J)";
            }

            if (eventArgs.Column.Header.ToString() == "EPEK")
            {
                eventArgs.Column.Header = "Ep + Ek (J)";
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

        public Visibility ButtonVisibility => (_difficulty == DifficultyOption.Easy)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}
