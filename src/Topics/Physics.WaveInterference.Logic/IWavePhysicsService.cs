namespace Physics.WaveInterference.Logic
{
	public interface IWavePhysicsService
	{
		float? CalculateY(float x, float time);

		float StartX { get; }

		float EndX { get; }

		WaveInfo Wave { get; }
	}
}
