using Physics.Shared.Mathematics;

namespace Physics.GravitationalFieldMovement.Logic;

public static class PlanetPresets
{
	public static PlanetPreset[] Presets { get; } = new[]
	{
		Earth,
		new PlanetPreset("Preset_Moon", new BigNumber(1.74, 6), new BigNumber(7.35, 22), "#627480"),
		new PlanetPreset("Preset_Mars", new BigNumber(3.39, 6), new BigNumber(6.39, 23), "#662C18"),
		Sun,
		new PlanetPreset("Preset_Custom", new BigNumber(6.38, 6), new BigNumber(5.97, 24), "#333333", false),
	};

	public static PlanetPreset Earth { get; } = new PlanetPreset(
		"Preset_Earth",
		new BigNumber(6.38, 6),
		new BigNumber(5.97, 24),
		"#2A6587");

	public static PlanetPreset Sun { get; } = new PlanetPreset(
			"Preset_Sun",
			new BigNumber(6.96, 8),
			new BigNumber(1.99, 30),
			"#EBB446");
		
}
