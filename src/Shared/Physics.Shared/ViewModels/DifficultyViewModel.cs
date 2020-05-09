using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.UI.Infrastructure.Topics;

namespace Physics.Shared.ViewModels
{
    public abstract class DifficultyViewModel : ViewModelBase<DifficultyOption>
    {
        public override void Prepare(DifficultyOption parameter) => Difficulty = parameter;

        public DifficultyOption Difficulty { get; private set; }
    }
}
