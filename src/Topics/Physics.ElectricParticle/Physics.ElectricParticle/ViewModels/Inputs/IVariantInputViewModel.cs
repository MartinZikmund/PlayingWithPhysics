using Physics.ElectricParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
    public interface IVariantInputViewModel
    {
        Task<IMotionSetup> CreateMotionSetupAsync();
        string Label { get; set; }
    }
}
