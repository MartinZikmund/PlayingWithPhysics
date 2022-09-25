using MvvmCross.ViewModels;

namespace Physics.WaveInterference.Game;

public class GameWaveInfo : MvxNotifyPropertyChanged
{
	public GameWaveInfo()
	{		
	}

	public GameWaveInfo(float f, float k)
	{
		F = f;
		K = k;
	}

	public float F { get; set; }

	public float K { get; set; }
}
