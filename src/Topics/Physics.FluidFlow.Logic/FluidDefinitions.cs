namespace Physics.FluidFlow.Logic
{
	public static class FluidDefinitions
	{
		public static FluidDefinition[] Definitions { get; } = new FluidDefinition[]
		{
			("Water", 1000f),
			("Oil", 830f),
			("HumanBlood", 1060f),
		};
	}
}
