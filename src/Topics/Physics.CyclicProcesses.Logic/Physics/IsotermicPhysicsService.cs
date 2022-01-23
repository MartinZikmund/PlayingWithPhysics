using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public class IsotermicPhysicsService : PhysicsService
{
	private readonly IsotermicInputConfiguration _input;
	private readonly float _nRT;

	public IsotermicPhysicsService(IsotermicInputConfiguration input) : base(input)
	{
		_input = input;
		_nRT = input.N * R * input.T;
		W12 = _nRT * (float)Math.Log(_input.V2 / _input.V1);
	}

	public override ProcessType Process => ProcessType.Isotermic;

	public float W12 { get; }

	public override float CalculateP(float time) => _nRT / CalculateV(time);

	public override float CalculateV(float time) => CalculateCycleValue(_input.V1, _input.V2, time);

	public float CalculateW(float time) => _nRT * (float)Math.Log(CalculateV(time) / _input.V1);
}
