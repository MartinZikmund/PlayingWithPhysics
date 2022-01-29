namespace Physics.CyclicProcesses.Logic.Physics
{
	public interface IPhysicsService
	{
		ProcessType Process { get; }

		float CalculateV(float time);

		float CalculateP(float time);

		float MinV { get; }

		float MinP { get; }

		float MaxV { get; }

		float MaxP { get; }
	}
}
