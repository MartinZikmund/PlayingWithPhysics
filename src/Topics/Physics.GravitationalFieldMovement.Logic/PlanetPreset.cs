using System.Drawing;
using Physics.Shared.Mathematics;

namespace Physics.GravitationalFieldMovement.Logic;

public class PlanetPreset
{
	public PlanetPreset(string nameKey, BigNumber r, BigNumber m, string colorHex)
	{
		NameKey = nameKey;
		R = r;
		M = m;
		ColorHex = colorHex;
	}

	public string NameKey { get; }

	public BigNumber R { get; }

	public BigNumber M { get; }
	public string ColorHex { get; }
}
