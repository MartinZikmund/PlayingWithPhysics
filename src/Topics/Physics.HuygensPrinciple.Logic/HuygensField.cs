using System;

namespace Physics.HuygensPrinciple.Logic
{
	public class HuygensField
	{
		private CellState[,] _field;

		public HuygensField(int width, int height)
		{
			_field = new CellState[width, height];
			Width = width;
			Height = height;
		}

		public CellState this[int x, int y]
		{
			get => _field[x, y];
			set => _field[x, y] = value;
		}

		public int Width { get; }

		public int Height { get; }

		public void Clear() => Array.Clear(_field, 0, _field.Length);

		public HuygensField Clone()
		{
			var clone = new HuygensField(Width, Height);
			Array.Copy(_field, clone._field, _field.Length);
			return clone;
		}
	}
}
