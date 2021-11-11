namespace Physics.StationaryWaves.Logic
{
	public interface IWavePhysicsService
	{
		float CalculateFirstWaveY(float x, float time);
		float CalculateSecondWaveY(float x, float time);

	}
}
