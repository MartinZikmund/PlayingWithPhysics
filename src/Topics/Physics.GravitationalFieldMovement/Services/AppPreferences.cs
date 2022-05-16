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
	
	public LengthUnit LengthUnit
	{
		get => _preferences.GetSetting(nameof(LengthUnit), () => LengthUnit.Metric);
		set => _preferences.SetSetting(nameof(LengthUnit), value);
	}
}
