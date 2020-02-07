using MvvmCross.ViewModels;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousMovement.ViewModels
{
    public class MovementInfoViewModel : MvxNotifyPropertyChanged
    {
        public MovementInfoViewModel(ThrowInfo throwInfo)
        {
            ThrowInfo = throwInfo;
        }

        public ThrowInfo ThrowInfo { get; }

        public string Label
        {
            get => ThrowInfo.Label; 
            set
            {
                ThrowInfo.Label = value;
                RaisePropertyChanged();
            }
        }
    }
}
