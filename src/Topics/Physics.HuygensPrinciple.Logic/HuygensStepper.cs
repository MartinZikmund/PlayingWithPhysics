using System;
using System.Collections.Generic;
using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public class HuygensStepper
	{
		private readonly Point[] _neighborCoordinates = new Point[]
		{
			new Point(0, 1),
			new Point(0, -1),
			new Point(1, 0),
			new Point(-1, 0)
		};

		private readonly int _fieldWidth;
		private readonly int _fieldHeight;
		private readonly int _stepRadius;

		private readonly List<StepInfo> _stepsCache = new List<StepInfo>();

		private CellState[,] _originalField;
		private CellState[,] _currentField;

		public HuygensStepper(CellState[,] field, int stepRadius)
		{
			_originalField = field;
			_fieldWidth = _originalField.GetLength(0);
			_fieldHeight = _originalField.GetLength(1);
			_stepRadius = stepRadius;
		}

		public HuygensStepper(int x, int y, int stepRadius) : this(new CellState[x, y], stepRadius)
		{
		}

		public int CurrentStep { get; set; } = 0;

		public CellState[,] CurrentField => _currentField;

		public void SaveOriginalField()
		{
			for (int x = 0; x < _fieldWidth; x++)
			{
				for (int y = 0; x < _fieldHeight; x++)
				{
					if (_currentField[x, y] == CellState.Wave ||
						_currentField[x, y] == CellState.WaveEdge)
					{
						_currentField[x, y] = CellState.Empty;
					}
				}
			}

			// Store as original and make a copy for state.
			_originalField = _currentField;
			_stepsCache.Clear();

			ResetField();
		}

		public void ClearField()
		{
			Array.Clear(_originalField, 0, _originalField.Length);
			ResetField();
		}

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

		public IList<Point> GetBorderPoints(CellState spotState, CellState backgroundState = 0)
		{
			var results = new List<Point>();
			for (int x = 0; x < _fieldWidth - 1; x++)
			{
				for (int y = 0; y < _fieldHeight - 1; y++)
				{
					if (_currentField[x, y] == spotState)
					{
						bool hasBackgroundNeighbor = false;
						for (int i = 0; i < _neighborCoordinates.Length; i++)
						{
							var point = new Point(x + _neighborCoordinates[i].X, y + _neighborCoordinates[i].Y);

							if (point.X < 0 ||
								point.Y < 0 ||
								point.X >= _fieldWidth ||
								point.Y >= _fieldHeight)
							{
								continue;
							}

							if (_currentField[point.X, point.Y] == backgroundState)
							{
								hasBackgroundNeighbor = true;
								break;
							}
						}

						if (hasBackgroundNeighbor)
						{
							results.Add(new Point(x, y));
						}
					}
				}
			}

			return results;
		}

		public Point[] GetBorderLayer(CellState spot, CellState background = CellState.Empty)
		{
			return null;
		}

		public CellStateChange[] NextStep()
		{
			if (CurrentStep < _stepsCache.Count)
			{
				PerformCachedStep(CurrentStep);
				CurrentStep++;
				return _stepsCache[CurrentStep - 1].CellStateChanges;
			}

			var spotState = CellState.Wave;
			if (CurrentStep == 0)
			{
				spotState = CellState.Source;
			}

			var currentSources = GetBorderPoints(spotState);

			var allChanges = new List<CellStateChange>();
			foreach (var source in currentSources)
			{
				var changes = PutCircle(source, _stepRadius, CellState.Wave);
				allChanges.AddRange(changes);
			}

			// Cache step
			var allChangesArray = allChanges.ToArray();
			_stepsCache.Add(new StepInfo(allChangesArray));

			return allChangesArray;
		}

		public void SetStep(int step)
		{
			ResetField();

			for (int i = 0; i < step; i++)
			{
				NextStep();
			}
		}

		private void PerformCachedStep(int step)
		{
			if (step >= _stepsCache.Count)
			{
				throw new InvalidOperationException("This step is not cached yet.");
			}

			var stepInfo = _stepsCache[step];
			foreach (var cellStateChange in stepInfo.CellStateChanges)
			{
				_currentField[cellStateChange.X, cellStateChange.Y] = cellStateChange.NewState;
			}
		}

		private void ResetField()
		{
			_currentField = new CellState[_fieldWidth, _fieldHeight];
			Array.Copy(_originalField, _currentField, _originalField.Length);

			// We are at the beginning.
			CurrentStep = 0;
		}
	}
}
