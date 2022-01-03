namespace Physics.StationaryWaves.Logic
{
	public interface IWavePhysicsService
	{
		public float? CalculateCompoundY(float x, float time);

		float? CalculateFirstWaveY(float x, float time);

		float? CalculateSecondWaveY(float x, float time);

		float? CalculateWavePackageY(float x, float time);

		float Amplitude { get; }

		float MinX { get; }

		float MaxX { get; }

		float WaveLength { get; }

		bool HasWavePackage { get; }
	}
}
