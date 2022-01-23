using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public class StirlingEnginePhysicsService : PhysicsService
{
	private readonly StirlingEngineInputConfiguration _input;

	public StirlingEnginePhysicsService(StirlingEngineInputConfiguration input) : base(input)
	{
		_input = input;
		P1 = _input.N * R * _input.T12 / _input.V1;
		P2 = _input.N * R * _input.T12 / _input.V2;
		P3 = _input.N * R * _input.T34 / _input.V2;
		P4 = _input.N * R * _input.T34 / _input.V1;
		W = _input.N * R * (_input.T12 * (float)Math.Log(P1 / P2) - _input.T34 * (float)Math.Log(P4 / P3));
		QIn = _input.N * Cv * (_input.T12 - _input.T34) + _input.N * R * _input.T12 * (float)Math.Log(P1 / P2);
		QOut = _input.N * Cv * (_input.T12 - _input.T34) + _input.N * R * _input.T34 * (float)Math.Log(P4 / P3);
		EffectiveEfficiency = W / QIn;
		TermicEfficiency = 1 - _input.T34 / _input.T12;
	}

	public float P1 { get; }

	public float P2 { get; }

	public float P3 { get; }

	public float P4 { get; }

	public float W { get; }

	public float QIn { get; }

	public float QOut { get; }

	public float EffectiveEfficiency { get; }

	public float TermicEfficiency { get; }

	public override ProcessType Process => ProcessType.StirlingEngine;

	public override float CalculateP(float time) => _input.N * R * CalculateT(time) / CalculateV(time);

	public override float CalculateV(float time) => CalculateCycleValue(_input.V1, _input.V2, time);

	public float CalculateT(float time) => CalculateCycleValue(_input.T12, _input.T34, time);
}
