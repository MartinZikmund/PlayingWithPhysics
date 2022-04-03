using System;
using Physics.Shared.Logic.Geometry;

namespace Physics.FluidFlow.Logic;

public class BernoulliWithHeightChangePhysicsService : PhysicsServiceBase, IPhysicsService
{
	private readonly SceneConfiguration _input;

	public BernoulliWithHeightChangePhysicsService(SceneConfiguration input)
	{
		_input = input;
	}

	public int ParticleCount => 3;

	public float XMax
	{
		get
		{
			if (_input.Fluid == FluidDefinitions.Water)
			{
				return 0.50f;
			}
			else if (_input.Fluid == FluidDefinitions.Oil)
			{
				return 5f;
			}
			else
			{
				throw new InvalidOperationException("Unsupported fluid");
			}
		}
	}

	public float YMin => 0;

	public float YMax => GetTotalHeight();

	public float V2 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerV2(),
			DiameterRelationType.S2Larger => CalculateS2LargerV2(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float T1 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerT1(),
			DiameterRelationType.S2Larger => CalculateS2LargerT1(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float T2 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerT2(),
			DiameterRelationType.S2Larger => CalculateS2LargerT2(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float T3 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerT3(),
			DiameterRelationType.S2Larger => CalculateS2LargerT3(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float P2 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerP2(),
			DiameterRelationType.S2Larger => CalculateS2LargerP2(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float H1 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerH1(),
			DiameterRelationType.S2Larger => CalculateS2LargerH1(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float H2 =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.S1Larger => CalculateS1LargerH2(),
			DiameterRelationType.S2Larger => CalculateS2LargerH2(),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public float DeltaP => Math.Abs(_input.Pressure - P2);

	public override float MaxT
	{
		get
		{
			if (_input.DiameterRelationType == DiameterRelationType.Equal)
			{
				return 60;
			}
			else if (_input.DiameterRelationType == DiameterRelationType.S1Larger)
			{
				return CalculateS1LargerT1() + CalculateS1LargerT2() + CalculateS1LargerT3();
			}
			else if (_input.DiameterRelationType == DiameterRelationType.S2Larger)
			{
				return CalculateS2LargerT1() + CalculateS2LargerT2() + CalculateS2LargerT3();
			}

			throw new InvalidOperationException("Invalid diameter type");
		}
	}

	public bool CanRenderFlow => throw new NotImplementedException();

	public string ErrorKey => throw new NotImplementedException();

	public float Vector1T => T1 / 2;

	public float Vector2T => T1 + T2 + T3 / 2;

	public float SimulationTimeAdjustment =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.Equal => 0.5f,
			DiameterRelationType.S1Larger => 0.01f,
			DiameterRelationType.S2Larger => 0.01f,
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	public Point2d GetParticlePosition(float time, int particleId) =>
		_input.DiameterRelationType switch
		{
			DiameterRelationType.Equal => GetDiameterEqualParticlePosition(time, particleId),
			DiameterRelationType.S1Larger => GetS1LargerParticlePosition(time, particleId),
			DiameterRelationType.S2Larger => GetS2LargerParticlePosition(time, particleId),
			_ => throw new InvalidOperationException("Invalid diameter type"),
		};

	private Point2d GetDiameterEqualParticlePosition(float time, int particleId)
	{
		var x = _input.Velocity * time;
		return particleId switch
		{
			0 => new Point2d(x, +_input.Diameter1 / 4),
			1 => new Point2d(x, 0),
			2 => new Point2d(x, -_input.Diameter1 / 4),
			_ => throw new InvalidOperationException()
		};
	}

	#region S1 Larger

	private Point2d GetS1LargerParticlePosition(float time, int particleId)
	{
		var t1 = T1;
		var t2 = T2;
		var v2 = V2;
		if (time < t1)
		{
			var x = _input.Velocity * time;
			return particleId switch
			{
				0 => new Point2d(x, +_input.Diameter1 / 4),
				1 => new Point2d(x, 0),
				2 => new Point2d(x, -_input.Diameter1 / 4),
				_ => throw new InvalidOperationException()
			};
		}
		else if (time < t1 + t2)
		{
			var x = 2 / 5f * XMax + 1 / 2f * (time - t1) * (v2 - _input.Velocity);
			var particle0y = _input.Diameter1 / 4 - ((x - 2 / 5f * XMax) * 1.2f * ((_input.Diameter1 - _input.Diameter2) / XMax));
			var particle2y = -_input.Diameter1 / 4 + ((x - 2 / 5f * XMax) * 1.2f * ((_input.Diameter1 - _input.Diameter2) / XMax));
			return particleId switch
			{
				0 => new Point2d(x, particle0y),
				1 => new Point2d(x, 0),
				2 => new Point2d(x, particle2y),
				_ => throw new InvalidOperationException()
			};
		}
		else
		{
			time = Math.Min(time, MaxT);
			var x = v2 * (time - t1 - t2) + 3 / 5f * XMax;
			return particleId switch
			{
				0 => new Point2d(x, +_input.Diameter2 / 4),
				1 => new Point2d(x, 0),
				2 => new Point2d(x, -_input.Diameter2 / 4),
				_ => throw new InvalidOperationException()
			};
		}
	}

	public float GetS1LargerX1() => _input.Fluid == FluidDefinitions.Water ? 0.20f : 2;

	public float GetS1LargerX2() => _input.Fluid == FluidDefinitions.Water ? 0.3f : 3;

	public float GetS1LargerX3() => _input.Fluid == FluidDefinitions.Water ? 0.5f : 5;

	public float CalculateS1LargerV2() => (_input.Velocity * _input.Diameter1 * _input.Diameter1) / (_input.Diameter2 * _input.Diameter2);

	public float CalculateS1LargerT1() => 2 * XMax / (5 * _input.Velocity);

	public float CalculateS1LargerT2() => 2 * XMax / (5 * (CalculateS1LargerV2() - _input.Velocity));

	public float CalculateS1LargerT3() => 2 * XMax / (5 * CalculateS1LargerV2());

	public float CalculateS1LargerP2() => _input.Pressure + 500 * (_input.Velocity * _input.Velocity - V2 * V2) + 100 * (_input.HeightDecrease + _input.Diameter1 / 2 + _input.Diameter2 / 2);

	public float CalculateS1LargerH1() => Math.Abs(_input.Pressure / 5000000);

	public float CalculateS1LargerH2() => H1 - 3 * DeltaP / 5000000;

	#endregion

	#region S2 Larger

	private Point2d GetS2LargerParticlePosition(float time, int particleId)
	{
		var t1 = T1;
		var t2 = T2;
		var v2 = V2;
		if (time < t1)
		{
			var x = _input.Velocity * time;
			return particleId switch
			{
				0 => new Point2d(x, +_input.Diameter1 / 4),
				1 => new Point2d(x, 0),
				2 => new Point2d(x, -_input.Diameter1 / 4),
				_ => throw new InvalidOperationException()
			};
		}
		else if (time < t1 + t2)
		{
			var x = 2 / 5f * XMax + 1 / 2f * (time - t1) * (v2 + _input.Velocity);
			var particle0y = _input.Diameter1 / 4 - ((x - 2 / 5f * XMax) * 1.2f * ((_input.Diameter1 - _input.Diameter2) / XMax));
			var particle2y = -_input.Diameter1 / 4 + ((x - 2 / 5f * XMax) * 1.2f * ((_input.Diameter1 - _input.Diameter2) / XMax));
			return particleId switch
			{
				0 => new Point2d(x, particle0y),
				1 => new Point2d(x, 0),
				2 => new Point2d(x, particle2y),
				_ => throw new InvalidOperationException()
			};
		}
		else
		{
			time = Math.Min(time, MaxT);
			var x = v2 * (time - t1 - t2) + 3 / 5f * XMax;
			return particleId switch
			{
				0 => new Point2d(x, +_input.Diameter2 / 4),
				1 => new Point2d(x, 0),
				2 => new Point2d(x, -_input.Diameter2 / 4),
				_ => throw new InvalidOperationException()
			};
		}
	}

	public float GetS2LargerX1() => 0.20f;

	public float GetS2LargerX2() => 0.3f;

	public float GetS2LargerX3() => 0.5f;

	public float CalculateS2LargerV2() => (_input.Velocity * _input.Diameter1 * _input.Diameter1) / (_input.Diameter2 * _input.Diameter2);

	public float CalculateS2LargerT1() => 2 * XMax / (5 * _input.Velocity);

	public float CalculateS2LargerT2() => 2 * XMax / (5 * (CalculateS2LargerV2() + _input.Velocity));

	public float CalculateS2LargerT3() => 2 * XMax / (5 * CalculateS2LargerV2());

	public float CalculateS2LargerP2() => _input.Pressure + 500 * (_input.Velocity * _input.Velocity - V2 * V2);

	public float CalculateS2LargerH1() => H2 - 3 * DeltaP / 5000000;

	public float CalculateS2LargerH2() => Math.Abs(P2 / 5000000);

	#endregion

	private float GetTotalHeight()
	{
		if (_input.DiameterRelationType == DiameterRelationType.S1Larger)
		{
			return _input.Diameter1 + _input.HeightDecrease;
		}
		else
		{
			return _input.Diameter2 + _input.HeightDecrease;
		}
	}
}
