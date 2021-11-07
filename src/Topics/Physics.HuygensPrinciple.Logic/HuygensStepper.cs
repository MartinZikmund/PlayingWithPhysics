using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

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

		private readonly int _stepRadius;

		private readonly List<StepInfo> _stepsCache = new List<StepInfo>();

		private HuygensField _originalField;

		public HuygensStepper(HuygensField field, int stepRadius)
		{
			_originalField = field;
			_stepRadius = stepRadius;
		}

		public int StepsAvailable => _stepsCache.Count;

		public StepInfo GetStep(int step)
		{
			if (step >= _stepsCache.Count)
			{
				throw new InvalidOperationException("This step is not yet available");
			}

			return _stepsCache[step];
		}

		public Task PrecalculateStepsAsync(CancellationToken token = default) =>
			Task.Run(() => PrecalculateSteps(token));

		private void PrecalculateSteps(CancellationToken token)
		{
			var huygensField = _originalField.Clone();
			StepInfo step;
			do
			{
				step = NextStep(huygensField, _stepsCache.Count == 0);
				_stepsCache.Add(step);
			} while (step.CellStateChanges.Length > 0);
		}

		private IList<Point> GetBorderPoints(HuygensField field, CellState spotState, CellState backgroundState = 0)
		{
			var results = new List<Point>();
			for (int x = 0; x < _originalField.Width - 1; x++)
			{
				for (int y = 0; y < _originalField.Height - 1; y++)
				{
					if (field[x, y] == spotState)
					{
						bool hasBackgroundNeighbor = false;
						for (int i = 0; i < _neighborCoordinates.Length; i++)
						{
							var point = new Point(x + _neighborCoordinates[i].X, y + _neighborCoordinates[i].Y);

							if (point.X < 0 ||
								point.Y < 0 ||
								point.X >= field.Width ||
								point.Y >= field.Height)
							{
								continue;
							}

							if (field[point.X, point.Y] == backgroundState)
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

		private Point[] GetBorderLayer(CellState spot, CellState background = CellState.Empty)
		{
			return null;
		}

		private StepInfo NextStep(HuygensField field, bool firstStep)
		{
			var spotState = CellState.Wave;
			if (firstStep)
			{
				spotState = CellState.Source;
			}

			var currentSources = GetBorderPoints(field, spotState);

			var allChanges = new List<CellStateChange>();
			foreach (var source in currentSources)
			{
				var changes = HuygensShapeDrawer.DrawCircle(field, source, _stepRadius, CellState.Wave);
				allChanges.AddRange(changes);
			}

			return new StepInfo(allChanges.ToArray());
		}
	}
}
