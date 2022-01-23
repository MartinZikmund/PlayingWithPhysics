namespace Physics.CyclicProcesses.Logic.Input;

public class IsochoricInputConfiguration : IInputConfiguration
{
	public IsochoricInputConfiguration(
		int n,
		float v,
		float t1,
		float t2)
	{
		N = n;
		V = v;
		T1 = t1;
		T2 = t2;
	}

	/// <summary>
	/// Input variant.
	/// </summary>
	public ProcessType Process => ProcessType.Isochoric;

	/// <summary>
	/// Volume.
	/// </summary>
	public float V { get; }

	/// <summary>
	/// Lower temperature.
	/// </summary>
	public float T1 { get; }

	/// <summary>
	/// Upper temperature.
	/// </summary>
	public float T2 { get; }

	/// <summary>
	/// Number of mols.
	/// </summary>
	public int N { get; set; }
}

