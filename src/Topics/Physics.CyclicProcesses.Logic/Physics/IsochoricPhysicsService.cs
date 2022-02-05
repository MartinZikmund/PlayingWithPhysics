using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public class IsochoricPhysicsService : PhysicsService, IBasicProcessPhysicsService
{
	private readonly IsochoricInputConfiguration _input;

	public IsochoricPhysicsService(IsochoricInputConfiguration input) : base(input)
	{
		_input = input;
		Q12 = _input.N * Cv * (_input.T2 - _input.T1);
	}

	public override float MinP => _input.N * R * Math.Min(_input.T1, _input.T2) / _input.V;

	public override float MaxP => _input.N * R * Math.Max(_input.T1, _input.T2) / _input.V;

	public override float MinV => _input.V;

	public override float MaxV => _input.V;

	public float Q12 { get; }

	public override ProcessType Process => ProcessType.Isochoric;

	public override float CalculateP(float time) => _input.N * R * CalculateT(time) / _input.V;

	public override float CalculateV(float time) => _input.V;

	public float CalculateT(float time) => CalculateCycleValue(_input.T1, _input.T2, time);

	public float CalculateQ(float time) => _input.N * Cv * (CalculateT(time) - _input.T1);

	public float CalculateW(float time) => 0;

	public float CalculateDeltaU(float time) => CalculateQ(time);
}
