namespace Physics.CyclicProcesses.Logic.Input;

public class IsobaricInputConfiguration : IInputConfiguration
{
	public IsobaricInputConfiguration(
		float n,
		float p,
		float v1,
		float v2)
	{
		N = n;
		P = p;
		V1 = v1;
		V2 = v2;
	}

	/// <summary>
	/// Input variant.
	/// </summary>
	public ProcessType Process => ProcessType.Isobaric;

	/// <summary>
	/// Pressure.
	/// </summary>
	public float P { get; }

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

