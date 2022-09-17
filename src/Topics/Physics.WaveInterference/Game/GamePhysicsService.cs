using System;

namespace Physics.WaveInterference.Game;

public class GamePhysicsService
{
	private readonly GameWaveInfo _wave1;
	private readonly GameWaveInfo _wave2;

	public GamePhysicsService(GameWaveInfo wave1, GameWaveInfo wave2)
	{
		_wave1 = wave1;
		_wave2 = wave2;
	}

	public double CalculateSingleWave(float x, float time, GameWaveInfo wave) => Math.Cos(2 * Math.PI * wave.F * time - wave.K * x);

	public double CalculateH(float x, float time)
	{
		var f = CalculateSingleWave(x, time, _wave1);
		var g = CalculateSingleWave(x, time, _wave2);
		return f + g;
	}

	public double CalculateAbsPackage(float x, float time) => 2 * Math.Cos(0.5 * (2 * Math.PI * (_wave1.F - _wave2.F) * time - (_wave1.K - _wave2.K) * x));

	public double CalculateVf() => Math.PI * (_wave1.F + _wave2.F) / (_wave1.K + _wave2.K);
	
	public double CalculateVg() => Math.PI * (_wave1.F - _wave2.F) / (_wave1.K - _wave2.K);
}
