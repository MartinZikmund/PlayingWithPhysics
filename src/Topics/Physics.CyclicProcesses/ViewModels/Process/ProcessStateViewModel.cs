using System;
using MvvmCross.ViewModels;
using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.ViewModels.Process
{
	public abstract class ProcessStateViewModel : MvxNotifyPropertyChanged
	{
		public ProcessStateViewModel(IInputConfiguration inputConfiguration)
		{
			Input = inputConfiguration;
		}

		protected IInputConfiguration Input { get; }

		internal static ProcessStateViewModel Create(IInputConfiguration input) =>
			input switch
			{
				IsotermicInputConfiguration config => new IsotermicStateViewModel(config),
				IsochoricInputConfiguration config => new IsochoricStateViewModel(config),
				IsobaricInputConfiguration config => new IsobaricStateViewModel(config),
				AdiabaticInputConfiguration config => new AdiabaticStateViewModel(config),
				StirlingEngineInputConfiguration config => new StirlingEngineStateViewModel(config),
				_ => throw new InvalidOperationException("Invalid process type"),
			};
	}
}
