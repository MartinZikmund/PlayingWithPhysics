namespace Physics.WaveInterference.Logic
{
	public interface IOscillationTrajectory
    {
		float GetY(float timeInSeconds, bool accurate = false);
    }
}
