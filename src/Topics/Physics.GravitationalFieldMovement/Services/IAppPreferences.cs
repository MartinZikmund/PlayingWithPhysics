using Physics.GravitationalFieldMovement.Logic;

namespace Physics.GravitationalFieldMovement.Services;

public interface IAppPreferences
{
	public LengthUnit LengthUnit { get; set; }
}
