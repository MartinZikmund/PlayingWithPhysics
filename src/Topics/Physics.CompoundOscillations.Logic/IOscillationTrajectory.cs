namespace Physics.CompoundOscillations.Logic
{
	public interface IOscillationTrajectory
    {
		float GetY(float timeInSeconds, bool accurate = false);
    }
}
