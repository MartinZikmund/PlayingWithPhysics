namespace Physics.WaveInterference.Game;

public static class GameTasks
{
	public static GameTask[] All { get; } = new GameTask[]
	{
		new("$v_f < v_g$", (vf, vg) => vf < vg),
		new("$v_f > v_g$", (vf, vg) => vf > vg),
		new("$v_f = v_g$", (vf, vg) => vf == vg),
		new("$v_g = 0$", (vf, vg) => vg == 0),
		new("$v_g < 0$", (vf, vg) => vg < 0),
	};
}
