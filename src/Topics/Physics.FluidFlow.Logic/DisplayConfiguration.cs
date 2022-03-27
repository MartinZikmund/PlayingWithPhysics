namespace Physics.FluidFlow.Logic;

public class DisplayConfiguration
{
	public DisplayConfiguration(InputVariant inputVariant, DiameterRelationType diameterRelationType)
	{
		InputVariant = inputVariant;
		DiameterRelationType = diameterRelationType;
	}

	public InputVariant InputVariant { get; }

	public DiameterRelationType DiameterRelationType { get; }

	public bool V { get; set; }

	public bool V1 { get; set; }

	public bool V2 { get; set; }

	public bool D { get; set; }

	public bool D1 { get; set; }

	public bool D2 { get; set; }

	public bool P { get; set; }

	public bool P1 { get; set; }

	public bool P2 { get; set; }

	public bool H1 { get; set; }

	public bool H2 { get; set; }
}
