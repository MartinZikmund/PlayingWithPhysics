namespace Physics.CyclicProcesses.Logic.Input;

public interface IInputConfiguration
{
	ProcessType Process { get; }

	int N { get; }
}
