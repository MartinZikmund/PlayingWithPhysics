namespace Physics.CyclicProcesses.Logic.Input;

public class IsotermicInputConfiguration : IInputConfiguration
{
	public IsotermicInputConfiguration(
		float n,
		float v1,
		float v2,
		float t)
	{
		N = n;
		T = t;
		V1 = v1;
		V2 = v2;
	}

	/// <summary>
	/// Input variant.
	/// </summary>
	public ProcessType Process => ProcessType.Isotermic;

	/// <summary>
	/// Temperature.
	/// </summary>
	public float T { get; }

	/// <summary>
	/// Lower volume.
	/// </summary>
	public float V1 { get; }

	/// <summary>
	/// Upper volume.
	/// </summary>
	public float V2 { get; }

	/// <summary>
	/// Number of mols.
	/// </summary>
	public float N { get; set; }
}
