using System;
using System.Linq;
using Physics.CyclicProcesses.Logic;
using Physics.Shared.ViewModels;

namespace Physics.CyclicProcesses.ViewModels
{
	public class DemoVariantViewModel : ViewModelBase
	{
		public ProcessType[] ProcessTypes { get; } = Enum.GetValues(typeof(ProcessType)).OfType<ProcessType>().ToArray();
	}
}
