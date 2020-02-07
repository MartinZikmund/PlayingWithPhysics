using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Physics.Shared.ViewModels;

namespace Physics.HomogenousMovement.ViewModels
{
    public class AddOrUpdateTrajectoryViewModel : ViewModelBase
    {
        public AddOrUpdateTrajectoryViewModel(string label)
        {
            Label = label;
        }
        
        public string Label { get; set; }
    }
}
