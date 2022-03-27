using Physics.FluidFlow.Logic;

namespace Physics.FluidFlow.ViewModels;

public class DisplayViewModel
{
	private readonly SceneConfiguration _sceneConfiguration;
	private readonly IPhysicsService _physicsService;

	public DisplayViewModel(SceneConfiguration sceneConfiguration)
	{
		_sceneConfiguration = sceneConfiguration;
		_physicsService = PhysicsServiceFactory.Create(sceneConfiguration);
	}

	public string V2 => _physicsService.V2.ToString("0.###");

	public string P2 => _physicsService.P2.ToString("0.###");

	public string H1 => _physicsService.H1.ToString("0.##");

	public string H2 => _physicsService.H2.ToString("0.##");
}
