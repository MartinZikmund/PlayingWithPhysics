using System.Drawing;
using Physics.Shared.Mathematics;

namespace Physics.GravitationalFieldMovement.Logic;

public class PlanetPreset
{
	public PlanetPreset(string nameKey, BigNumber r, BigNumber m, string colorHex, bool isReadOnly = true)
	{
		NameKey = nameKey;
		R = r;
		M = m;
		ColorHex = colorHex;
		IsReadOnly = isReadOnly;
	}

	public string NameKey { get; }

	public BigNumber R { get; set; }

	public BigNumber M { get; set; }
	
	public string ColorHex { get; }

	public bool IsReadOnly { get; }
}
