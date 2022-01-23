namespace Physics.CyclicProcesses.Logic.Physics
{
	public interface IBasicProcessPhysicsService
	{
		float CalculateV(float time);

		float CalculateP(float time);

		float CalculateT(float time);

		float CalculateW(float time);

		float CalculateQ(float time);

		float CalculateDeltaU(float time);
	}
}
