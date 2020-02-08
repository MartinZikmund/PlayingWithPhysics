using MvvmCross.ViewModels;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousMovement.ViewModels
{
    public class MotionInfoViewModel : MvxNotifyPropertyChanged
    {
        public MotionInfoViewModel(MotionInfo throwInfo)
        {
            MotionInfo = throwInfo;
        }

        public MotionInfo MotionInfo { get; set; }

        public string Label
        {
            get => MotionInfo.Label; 
            set
            {
                MotionInfo.Label = value;
                RaisePropertyChanged();
            }
        }
    }
}
