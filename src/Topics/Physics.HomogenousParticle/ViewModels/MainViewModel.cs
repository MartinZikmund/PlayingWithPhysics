using Physics.HomogenousParticle.Models;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ViewModels
{
    public class MainViewModel : ViewModelBase<MainViewModel.NavigationModel>
    {
        public class NavigationModel
        {
        }

        public MainViewModel()
        {
        }

        public override void Prepare(NavigationModel parameter)
        {
        }
        public VelocityVariant Variant { get; set; }
    }
}
