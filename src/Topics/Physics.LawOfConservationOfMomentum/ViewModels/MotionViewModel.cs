using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.LawOfConservationOfMomentum.Logic;

namespace Physics.LawOfConservationOfMomentum.ViewModels
{
	public class MotionViewModel
	{
		public MotionViewModel(MotionSetup setup)
		{
			Setup = setup;
		}

		public MotionSetup Setup { get; }
	}
}
