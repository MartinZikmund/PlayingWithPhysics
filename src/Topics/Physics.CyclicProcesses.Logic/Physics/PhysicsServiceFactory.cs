using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public static class PhysicsServiceFactory
{
	public static IPhysicsService GetPhysicsService(IInputConfiguration inputConfiguration) =>
		inputConfiguration switch
		{
			IsobaricInputConfiguration isobaricInput => new IsobaricPhysicsService(isobaricInput),
			IsochoricInputConfiguration isochoricInput => new IsochoricPhysicsService(isochoricInput),
			IsotermicInputConfiguration isotermicInput => new IsotermicPhysicsService(isotermicInput),
			AdiabaticInputConfiguration adiabaticInput => new AdiabaticPhysicsService(adiabaticInput),
			StirlingEngineInputConfiguration stirlingEngineInput => new StirlingEnginePhysicsService(stirlingEngineInput),
			_ => throw new InvalidOperationException("Unsupported input type"),
		};
}
