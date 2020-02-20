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

namespace Physics.HomogenousMovement.ViewModels
{
    public class ValuesTableDialogViewModel : INotifyPropertyChanged
    {
        private readonly IPhysicsService _service;
        public float TimeInterval { get; set; } = 0.1f;
        public ObservableCollection<TableRow> Values { get; private set; }
        private TableService _tableService;
        private MovementType _type;

        public event PropertyChangedEventHandler PropertyChanged;

        public ValuesTableDialogViewModel(IPhysicsService service, MovementType type)
        {
            _type = type;
            _service = service;
            _tableService = new TableService(service);
            Values = new ObservableCollection<TableRow>(_tableService.PopulateTable(TimeInterval));
        }

        public void OnTimeIntervalChanged()
        {
            Values = new ObservableCollection<TableRow>(_tableService.PopulateTable(TimeInterval));
        }

        public Visibility ButtonVisibility => (_type == MovementType.FreeFall || _type == MovementType.UpwardMotion)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
