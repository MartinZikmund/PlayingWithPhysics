namespace Physics.FluidFlow.Logic
{
	public static class FluidDefinitions
	{
		public static FluidDefinition[] Definitions { get; } = new FluidDefinition[]
		{
			("Air", 1.2f),
			("Oil", 830f),
			("Water", 1000f),
		};
	}
}
