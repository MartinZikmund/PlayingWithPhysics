using Physics.FluidFlow.Logic;
using Physics.Shared.Mathematics;

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

	public string V2 => _physicsService.V2.ToSignificantDigitsString(3);

	public string P2 => _physicsService.P2.ToSignificantDigitsString(3);

	public string H1 => _physicsService.H1.ToSignificantDigitsString(3);

	public string H2 => _physicsService.H2.ToSignificantDigitsString(3);

	public string Re => (_physicsService as RealFluidFlowPhysicsService)?.R.ToSignificantDigitsString(3);

	public string DeltaP => (_physicsService as RealFluidFlowPhysicsService)?.DeltaP.ToSignificantDigitsString(3);
}
