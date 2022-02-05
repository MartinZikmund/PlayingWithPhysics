using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public class IsobaricPhysicsService : PhysicsService, IBasicProcessPhysicsService
{
	private readonly IsobaricInputConfiguration _input;

	public IsobaricPhysicsService(IsobaricInputConfiguration input) : base(input)
	{
		_input = input;
		T1 = _input.P * _input.V1 / (_input.N * R);
		T2 = _input.P * _input.V2 / (_input.N * R);
		W12 = _input.P * (_input.V2 - _input.V1);
		Q12 = _input.N * Cp * (T2 - T1);
	}

	public float T1 { get; }

	public float T2 { get; }

	public float W12 { get; }

	public float U12 { get; }

	public float Q12 { get; }

	public override float MinP => _input.P;

	public override float MaxP => _input.P;

	public override float MinV => Math.Min(_input.V1, _input.V2);

	public override float MaxV => Math.Max(_input.V1, _input.V2);

	public override ProcessType Process => ProcessType.Isobaric;

	public override float CalculateP(float time) => _input.P;

	public override float CalculateV(float time) => CalculateCycleValue(_input.V1, _input.V2, time);

	public float CalculateT(float time) => _input.P * CalculateV(time) / (_input.N * R);

	public float CalculateW(float time) => _input.P * (CalculateV(time) - _input.V1);

	public float CalculateDeltaU(float time) => _input.N * Cv * (CalculateT(time) - T1);

	public float CalculateQ(float time) => _input.N * Cp * (CalculateT(time) - T1);
}
