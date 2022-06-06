using System;
using Physics.GravitationalFieldMovement.Logic;
using Physics.Shared.Services.Preferences;

namespace Physics.GravitationalFieldMovement.Services;

public class AppPreferences : IAppPreferences
{
	private readonly IPreferences _preferences;

	public AppPreferences(IPreferences preferences)
	{
		_preferences = preferences;
	}

	// Value should not be persisted as per https://github.com/Playing-With-Physics/03-Pohyby-v-centralnim-gravitacnim-poli/issues/41
	public LengthUnit LengthUnit { get; set; } = LengthUnit.Metric;
}
