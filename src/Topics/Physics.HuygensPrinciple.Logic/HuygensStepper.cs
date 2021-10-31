using System;
using System.Collections.Generic;
using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public class HuygensStepper
	{		
		private readonly int _fieldWidth;
		private readonly int _fieldHeight;
		private CellState[,] _originalField;
		private CellState[,] _currentField;

		public HuygensStepper(CellState[,] field)
		{
			_originalField = field;
			_fieldWidth = _originalField.GetLength(0);
			_fieldHeight = _originalField.GetLength(1);
		}

		public HuygensStepper(int x, int y)
		{
			_originalField = new CellState[x, y];
			_fieldWidth = x;
			_fieldHeight = y;
		}

		public int CurrentStep { get; set; } = 0;

		public CellState[,] CurrentField => _currentField;

		public void SaveOriginalField()
		{
			for (int x = 0; x < _fieldWidth; x++)
			{
				for (int y = 0; x < _fieldHeight; x++)
				{
					if (_currentField[x,y] == CellState.Wave ||
						_currentField[x,y] == CellState.WaveEdge)
					{
						_currentField[x, y] = CellState.Empty;
					}
				}
			}

			// Store as original and make a copy for state.
			_originalField = _currentField;
			_currentField = new CellState[_fieldWidth, _fieldHeight];
			Array.Copy(_originalField, _currentField, _originalField.Length);

			// We are at the beginning.
			CurrentStep = 0;
		}

		public void EmptyField() => Array.Clear(_originalField, 0, _originalField.Length);

		public IList<CellStateChange> PutRectangle(Point leftUpper, Point rightLower, CellState fill = CellState.Source, bool avoidObjects = true)
		{
			var results = new List<CellStateChange>();

			for (int x = leftUpper.X; x <= rightLower.X; x++)
			{
				for (int y = leftUpper.Y; y <= rightLower.Y; y++)
				{
					if (avoidObjects && (_originalField[x, y] == CellState.Source || _originalField[x, y] == CellState.Wall))
					{
						continue;
					}

					if (_originalField[x, y] != fill)
					{
						_originalField[x, y] = fill;
						results.Add(new CellStateChange(x, y, fill));
					}
				}
			}

			return results;
		}

		public IList<CellStateChange> PutCircle(Point center, float radius, CellState fill = CellState.Source, bool avoidObjects = true)
		{
			var results = new List<CellStateChange>();

			var (cX, cY) = (center.X, center.Y);
			var xStart = Math.Max((int)(cX - radius - 1), 0);
			var xEnd = Math.Min((int)(cX + radius + 1), _fieldWidth - 1);

			var yStart = Math.Max((int)(cY - radius - 1), 0);
			var yEnd = Math.Min((int)(cY + radius + 1), _fieldHeight - 1);

			var radius2 = radius * radius;

			for (int x = xStart; x <= xEnd; x++)
			{
				for (int y = yStart; y <= yEnd; y++)
				{
					if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= radius2)
					{
						if (avoidObjects && (_originalField[x, y] == CellState.Source || _originalField[x, y] == CellState.Wall))
						{
							continue;
						}

						if (_originalField[x, y] != fill)
						{
							_originalField[x, y] = fill;
							results.Add(new CellStateChange(x, y, fill));
						}
					}
				}
			}

			return results;
		}

		public Point[] GetBorderPoints(CellState spot, int background = 0)
		{

		}

		public CellStateChange[] NextStep()
		{

		}

		public int SetStep(int step)
		{

		}
	}
}
