namespace Physics.FluidFlow.Logic
{
	public static class FluidDefinitions
	{
		public static FluidDefinition Water { get; } = new FluidDefinition("Water", 1000f, "#D4F1F9");

		public static FluidDefinition Oil { get; } = new FluidDefinition("Oil", 830f, "#A98B2D");
	}
}
