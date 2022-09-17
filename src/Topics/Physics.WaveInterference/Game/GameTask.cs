using System;

namespace Physics.WaveInterference.Game;

public class GameTask
{
	private readonly Func<float, float, bool> _evaluator;

	public GameTask(string label, Func<float, float, bool> evaluator)
	{
		Label = label;
		_evaluator = evaluator;
	}

	public string Label { get; }

	public bool Evaluate(float vf, float vg) => _evaluator.Invoke(vf, vg);
}
