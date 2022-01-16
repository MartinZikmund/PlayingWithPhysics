namespace Physics.FluidFlow.Logic
{
	public class FluidDefinitions
	{
		public FluidDefinition[] Definitions { get; } = new FluidDefinition[]
		{
			("Air", 1.2f),
			("Oil", 830f),
			("Water", 1000f),
		};
	}
}
