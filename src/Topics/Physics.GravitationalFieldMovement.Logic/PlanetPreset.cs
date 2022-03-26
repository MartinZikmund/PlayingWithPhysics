using Physics.Shared.Mathematics;

namespace Physics.GravitationalFieldMovement.Logic;

public class PlanetPreset
{
	public PlanetPreset(string nameKey, BigNumber r, BigNumber m)
	{
		NameKey = nameKey;
		R = r;
		M = m;
	}

	public string NameKey { get; }

	public BigNumber R { get; }

	public BigNumber M { get; }
}
