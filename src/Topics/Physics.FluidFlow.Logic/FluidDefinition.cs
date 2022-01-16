namespace Physics.FluidFlow.Logic
{
	public class FluidDefinition
	{
		public FluidDefinition(string name, float density)
		{
			Name = name;
			Density = density;
		}

		public static implicit operator FluidDefinition((string name, float density) tuple) =>
			new FluidDefinition(tuple.name, tuple.density);

		public string Name { get; }

		public float Density { get; }
	}
}
