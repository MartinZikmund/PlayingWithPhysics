using System.Security.Cryptography;
using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public class AdiabaticPhysicsService : PhysicsService, IBasicProcessPhysicsService
{
	private readonly AdiabaticInputConfiguration _input;

	private readonly float _v1PowK;

	public AdiabaticPhysicsService(AdiabaticInputConfiguration input) : base(input)
	{
		_input = input;
		_v1PowK = (float)Math.Pow(_input.V1, K);
		var v2PowK = (float)Math.Pow(_input.V2, K);
		P2 = _input.P1 * _v1PowK / v2PowK;
		T1 = _input.P1 * _input.V1 / (_input.N * R);
		T2 = P2 * _input.V2 / (_input.N * R);
		W12 = P2 * _input.V2 - _input.P1 * _input.V1 / (K - 1);
	}

	public override ProcessType Process => ProcessType.Adiabatic;

	public float P2 { get; }

	public float T1 { get; }

	public float T2 { get; }

	public float W12 { get; }

	public float U12 => -W12;

	public override float CalculateP(float time)
	{
		var vPowK = (float)Math.Pow(CalculateV(time), K);
		return _input.P1 * _v1PowK / vPowK;
	}

	public override float CalculateV(float time) => CalculateCycleValue(_input.V1, _input.V2, time);

	public float CalculateT(float time) => CalculateP(time) * CalculateV(time) / (_input.N * R);

	public float CalculateW(float time) => CalculateP(time) * CalculateV(time) - _input.P1 * _input.V1 / (K - 1);

	public float CalculateQ(float time) => 0;

	public float CalculateDeltaU(float time) => -CalculateW(time);
}
