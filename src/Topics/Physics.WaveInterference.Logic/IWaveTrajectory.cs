namespace Physics.WaveInterference.Logic
{
	public interface IWaveTrajectory
    {
		float? GetY(float x, float time, bool accurate = false);

		float StartX { get; }

		float EndX { get; }
    }
}
