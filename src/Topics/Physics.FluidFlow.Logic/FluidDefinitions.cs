namespace Physics.FluidFlow.Logic
{
	public static class FluidDefinitions
	{
		public static FluidDefinition Water { get; } = new FluidDefinition("Water", 1000f);

		public static FluidDefinition Oil { get; } = new FluidDefinition("Oil", 1000f);
	}
}
