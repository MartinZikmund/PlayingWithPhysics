namespace Physics.CompoundOscillations.Logic
{
	public interface IOscillationPhysicsService
    {
		float CalculateY(float timeInSeconds);

		float CalculateA(float timeInSeconds);

		float CalculateV(float timeInSeconds);
	}
}
