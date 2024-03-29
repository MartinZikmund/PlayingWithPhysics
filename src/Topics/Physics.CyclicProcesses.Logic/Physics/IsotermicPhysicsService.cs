﻿using Physics.CyclicProcesses.Logic.Input;

namespace Physics.CyclicProcesses.Logic.Physics;

public class IsotermicPhysicsService : PhysicsService, IBasicProcessPhysicsService
{
	private readonly IsotermicInputConfiguration _input;
	private readonly float _nRT;

	public IsotermicPhysicsService(IsotermicInputConfiguration input) : base(input)
	{
		_input = input;
		_nRT = input.N * R * input.T;
		W12 = _nRT * (float)Math.Log(_input.V2 / _input.V1);
	}

	public override float MinP => _nRT / MaxV;

	public override float MaxP => _nRT / MinV;

	public override float MinV => Math.Min(_input.V1, _input.V2);

	public override float MaxV => Math.Max(_input.V1, _input.V2);

	public override ProcessType Process => ProcessType.Isotermic;

	public float W12 { get; }

	public override float CalculateP(float time) => _nRT / CalculateV(time);

	public float CalculateT(float time) => _input.T;

	public override float CalculateV(float time) => CalculateCycleValue(_input.V1, _input.V2, time);

	public float CalculateW(float time) => _nRT * (float)Math.Log(CalculateV(time) / _input.V1);

	public float CalculateQ(float time) => CalculateW(time);

	public float CalculateDeltaU(float time) => 0;
}
