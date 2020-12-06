using System;
using System.Linq;

namespace Physics.Shared.UI.Rendering
{
	public class TickDistance
	{
		private static readonly int[] _bases = new int[] { 1, 2, 5 };

		private int _baseIndex = 0;
		private float _multiplier = 0;

		public TickDistance(int baseNumber, float multiplier = 1f)
		{
			if (!_bases.Contains(baseNumber))
			{
				throw new InvalidOperationException("Base does not exist");
			}
			_baseIndex = Array.IndexOf(_bases, baseNumber);
			_multiplier = multiplier;
		}

		public void Increment()
		{
			if (_baseIndex == _bases.Length - 1)
			{
				_multiplier *= 10;
			}

			_baseIndex = _baseIndex + 1;
			_baseIndex %= _bases.Length;
		}

		public void Decrement()
		{
			if (_baseIndex == 0)
			{
				_multiplier /= 10;
			}

			_baseIndex = _baseIndex - 1;
			if (_baseIndex < 0)
			{
				_baseIndex += _bases.Length;
			}
		}

		public float Value => _multiplier * _bases[_baseIndex];
	}
}
