using System.Collections.Specialized;

namespace Physics.FluidFlow.Logic
{
	public class FluidDefinition
	{
		public FluidDefinition(string name, float density, string color)
		{
			Name = name;
			Density = density;
			Color = color;
		}

		public static implicit operator FluidDefinition((string name, float density, string color) tuple) =>
			new FluidDefinition(tuple.name, tuple.density, tuple.color);

		public string Name { get; }

		public float Density { get; }

		public string Color { get; }
	}
}
