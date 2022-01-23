namespace Physics.CyclicProcesses.Logic.Input;

public class AdiabaticInputConfiguration : IInputConfiguration
{
	public AdiabaticInputConfiguration(
		int n,
		float p1,
		float v1,
		float v2)
	{
		N = n;
		P1 = p1;
		V1 = v1;
		V2 = v2;
	}

	/// <summary>
	/// Input variant.
	/// </summary>
	public ProcessType Process => ProcessType.Adiabatic;

	/// <summary>
	/// Pressure.
	/// </summary>
	public float P1 { get; }

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
	public int N { get; set; }
}

