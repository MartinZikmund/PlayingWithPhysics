using Physics.ElectricParticle.ViewModels.Inputs;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Physics.ElectricParticle.Logic;

namespace Physics.ElectricParticle.ViewModels.Inputs
{
    public abstract class VariantInputViewModelBase : ViewModelBase, IVariantInputViewModel
    {
        private Color _color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1");

        public Color[] AvailableColors { get; } = new Color[]
        {
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#2D7D9A"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#E81123"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#881798"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#498205"),
            Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#515C6B"),
        };

        public Color Color
        {
            get => _color;
            set
            {
                if (value != _color)
                {
                    _color = value;
                    RaisePropertyChanged();
                }
            }
        }

        public abstract string Label { get; set; }

        public abstract Task<IMotionSetup> CreateMotionSetupAsync();
    }
}
