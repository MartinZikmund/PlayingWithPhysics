using System;

namespace Physics.LawOfConservationOfMomentum.Logic
{
	public class PhysicsService
	{
		private readonly MotionSetup _setup;

		private float? _displayWidth = null;
		private float? _x1Start = null;
		private float? _x2Start = null;
		private float? _collisionX = null;
		private float? _collisionTime = null;

		public PhysicsService(MotionSetup setup)
		{
			_setup = setup ?? throw new ArgumentNullException(nameof(setup));
		}

		public float GetDisplayWidth()
		{
			if (_displayWidth == null)
			{
				_displayWidth = _setup.Subtype switch
				{
					CollisionSubtype.V2ZeroM2BiggerThanM1 => _setup.V1 <= 10 ? 15 : 30,
					CollisionSubtype.V2Zero => _setup.V1 <= 10 ? 20 : 40,
					CollisionSubtype.SpeedsSameDirection => 160,
					CollisionSubtype.SpeedsOppositeDirection => 160,
					_ => throw new InvalidOperationException("Unknown combination of type and subtype"),
				};
			}

			return _displayWidth.Value;
		}

		public float GetX1Start()
		{
			if (_x1Start == null)
			{
				_x1Start = _setup.Subtype switch
				{
					CollisionSubtype.V2ZeroM2BiggerThanM1 => 0,
					CollisionSubtype.V2Zero => 0,
					CollisionSubtype.SpeedsSameDirection => 0,
					CollisionSubtype.SpeedsOppositeDirection => GetDisplayWidth() / 4,
					_ => throw new InvalidOperationException("Unknown combination of type and subtype"),
				};
			}

			return _x1Start.Value;
		}

		public float GetX2Start()
		{
			if (_x2Start == null)
			{
				_x2Start = _setup.Subtype switch
				{
					CollisionSubtype.V2ZeroM2BiggerThanM1 => 2 * GetDisplayWidth() / 3,
					CollisionSubtype.V2Zero => GetDisplayWidth() / 2,
					CollisionSubtype.SpeedsSameDirection => (_setup.V1 / _setup.V2) > 2.6 ? 100 : (float)16,
					CollisionSubtype.SpeedsOppositeDirection => GetDisplayWidth() * 3 / 4,
					_ => throw new InvalidOperationException("Unknown combination of type and subtype"),
				};
			}

			return _x2Start.Value;
		}

		public float GetDistance(float time)
		{
			var x1 = GetX1(time);
			var x2 = GetX2(time);

			return Math.Abs(x1 - x2);
		}

		public float GetCollisionX()
		{
			if (_collisionX == null)
			{
				_collisionX = _setup.Subtype switch
				{
					CollisionSubtype.V2ZeroM2BiggerThanM1 => 2 * GetDisplayWidth() / 3,
					CollisionSubtype.V2Zero => 2 * GetDisplayWidth() / 3,
					CollisionSubtype.SpeedsSameDirection => _setup.V1 * GetX2Start() / (_setup.V1 - _setup.V2), //TODO Verify
					CollisionSubtype.SpeedsOppositeDirection => _setup.V1 * GetX2Start() / (_setup.V1 + _setup.V2),
					_ => throw new InvalidOperationException("Invalid subtype"),
				};
			}

			return _collisionX.Value;
		}

		public float GetCollisionTime()
		{
			if (_collisionTime == null)
			{
				_collisionTime = _setup.Subtype switch
				{
					CollisionSubtype.V2ZeroM2BiggerThanM1 => GetCollisionX() / _setup.V1,
					CollisionSubtype.V2Zero => GetCollisionX() / _setup.V1,
					CollisionSubtype.SpeedsSameDirection => GetDistance(0) / (_setup.V1 - _setup.V2),
					CollisionSubtype.SpeedsOppositeDirection => GetDistance(0) / (_setup.V1 + _setup.V2),
					_ => throw new NotImplementedException("Invalid subtype"),
				};
			}

			return _collisionTime.Value;
		}

		public float GetX1(float time) =>
			time <= GetCollisionTime() ?
				GetX1BeforeCollision(time) : GetX1AfterCollision(time);

		public float GetX2(float time) =>
			time <= GetCollisionTime() ?
				GetX2BeforeCollision(time) : GetX2AfterCollision(time);

		public float GetV1(float time) =>
			time <= GetCollisionTime() ?
				_setup.V1 : GetV1AfterCollision();

		public float GetV2(float time) =>
			time <= GetCollisionTime() ?
				_setup.V2 : GetV2AfterCollision();

		public float GetV2BeforeCollision() => _setup.Subtype switch
		{
			CollisionSubtype.V2ZeroM2BiggerThanM1 => 0,
			CollisionSubtype.V2Zero => 0,
			CollisionSubtype.SpeedsSameDirection => _setup.V2,
			CollisionSubtype.SpeedsOppositeDirection => _setup.V2,
			_ => throw new NotImplementedException(),
		};

		public float GetV1AfterCollision()
		{
			switch (_setup.Type)
			{
				case CollisionType.PerfectlyElastic:
					return _setup.Subtype switch
					{
						CollisionSubtype.V2ZeroM2BiggerThanM1 => -_setup.V1,
						CollisionSubtype.V2Zero => (_setup.M1 * _setup.V1 - _setup.M2 * _setup.V1) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsSameDirection => (_setup.M1 * _setup.V1 + _setup.M2 * (2 * _setup.V2 - _setup.V1)) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsOppositeDirection => (_setup.M1 * _setup.V1 + _setup.M2 * (-2 * _setup.V2 - _setup.V1)) / (_setup.M1 + _setup.M2),
						_ => throw new NotImplementedException(),
					};
				case CollisionType.PerfectlyInelastic:
					return _setup.Subtype switch
					{
						CollisionSubtype.V2ZeroM2BiggerThanM1 => 0,
						CollisionSubtype.V2Zero => (_setup.M1 * _setup.V1) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsSameDirection => (_setup.M1 * _setup.V1 + _setup.M2 * _setup.V2) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsOppositeDirection => (_setup.M1 * _setup.V1 - _setup.M2 * _setup.V2) / (_setup.M1 + _setup.M2),
						_ => throw new NotImplementedException(),
					};
				case CollisionType.ImperfectlyElastic:
					return _setup.Subtype switch
					{
						CollisionSubtype.V2ZeroM2BiggerThanM1 => _setup.V1 * _setup.CoefficientOfRestitution,
						CollisionSubtype.V2Zero => (_setup.M1 * _setup.V1 - _setup.CoefficientOfRestitution * _setup.M2 * _setup.V1) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsSameDirection => (_setup.M1 * _setup.V1 + _setup.M2 * (_setup.V2 + _setup.CoefficientOfRestitution * _setup.V2 - _setup.CoefficientOfRestitution * _setup.V1)) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsOppositeDirection => (_setup.M1 * _setup.V1 + _setup.M2 * (-_setup.CoefficientOfRestitution * _setup.V1 - _setup.V2 - _setup.CoefficientOfRestitution * _setup.V2)) / (_setup.M1 + _setup.M2),
						_ => throw new NotImplementedException(),
					};
			}

			throw new InvalidOperationException("Invalid type");
		}

		public float GetV2AfterCollision()
		{
			switch (_setup.Type)
			{
				case CollisionType.PerfectlyElastic:
					return _setup.Subtype switch
					{
						CollisionSubtype.V2ZeroM2BiggerThanM1 => 0,
						CollisionSubtype.V2Zero => (2 * _setup.M1 * _setup.V1) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsSameDirection => (_setup.M2 * _setup.V2 + _setup.M1 * (2 * _setup.V1 - _setup.V2)) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsOppositeDirection => (-_setup.M2 * _setup.V2 + _setup.M1 * (2 * _setup.V1 + _setup.V2)) / (_setup.M1 + _setup.M2),
						_ => throw new NotImplementedException(),
					};
				case CollisionType.PerfectlyInelastic:
					return _setup.Subtype switch
					{
						CollisionSubtype.V2ZeroM2BiggerThanM1 => 0,
						CollisionSubtype.V2Zero => (_setup.M1 * _setup.V1) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsSameDirection => (_setup.M1 * _setup.V1 + _setup.M2 * _setup.V2) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsOppositeDirection => (_setup.M1 * _setup.V1 - _setup.M2 * _setup.V2) / (_setup.M1 + _setup.M2),
						_ => throw new NotImplementedException(),
					};
				case CollisionType.ImperfectlyElastic:
					return _setup.Subtype switch
					{
						CollisionSubtype.V2ZeroM2BiggerThanM1 => 0,
						CollisionSubtype.V2Zero => ((1 + _setup.CoefficientOfRestitution) * _setup.M1 * _setup.V1) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsSameDirection => (_setup.M2 * _setup.V2 + _setup.M1 * (_setup.V1 + _setup.CoefficientOfRestitution * _setup.V1 - _setup.CoefficientOfRestitution * _setup.V2)) / (_setup.M1 + _setup.M2),
						CollisionSubtype.SpeedsOppositeDirection => (-_setup.M2 * _setup.V1 + _setup.M1 * (_setup.V1 + _setup.CoefficientOfRestitution * _setup.V1 + _setup.CoefficientOfRestitution * _setup.V2)) / (_setup.M1 + _setup.M2),
						_ => throw new NotImplementedException(),
					};
			}

			throw new InvalidOperationException("Invalid type");
		}

		public float GetX1BeforeCollision(float time) => _setup.V1 * time;

		public float GetX2BeforeCollision(float time) => GetX2Start() - GetV2BeforeCollision() * time;

		public float GetX1AfterCollision(float time) => GetCollisionX() + GetV1AfterCollision() * (time - GetCollisionTime());

		public float GetX2AfterCollision(float time) => GetCollisionX() + GetV2AfterCollision() * (time - GetCollisionTime());
	}
}
