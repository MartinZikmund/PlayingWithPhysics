using Physics.ElectricParticle.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.ViewModels
{
    public interface IVariantStateViewModel
    {
        public IMotionSetup Motion { get; }
    }
}
