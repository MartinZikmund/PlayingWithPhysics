using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
			new Point(-1, 0),
			new Point(-1, -1),
			new Point(1, 1),
			new Point(-1, 1),
			new Point(1, -1),
		};

		private readonly float _stepRadius;

		private readonly List<StepInfo> _stepsCache = new List<StepInfo>();

		private HuygensField _originalField;

		public HuygensStepper(HuygensField field, float stepRadius)
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
			IList<Point> nextSteps = null;
			do
			{
				step = NextStep(huygensField, nextSteps);
				nextSteps = step.WaveBorder;
				_stepsCache.Add(step);
			} while (step.CellStateChanges.Length > 0);
		}

		public IList<Point> GetBorderPoints(HuygensField field, CellState spotState, CellState backgroundState = 0)
		{
			var results = new List<Point>();
			for (int x = 0; x < _originalField.Width - 1; x++)
			{
				for (int y = 0; y < _originalField.Height - 1; y++)
				{
					if (field[x, y] == spotState)
					{
						var point = new Point(x, y);
						if (HasBackgroundNeighbor(point, field, backgroundState))
						{
							results.Add(point);
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

		private StepInfo NextStep(HuygensField field, IList<Point> currentSources = null)
		{
			if (currentSources == null)
			{
				currentSources = GetBorderPoints(field, CellState.Source);
			}

			HashSet<Point> nextSources = new HashSet<Point>();

			var allChanges = new List<CellStateChange>();
			foreach (var source in currentSources)
			{
				var changes = HuygensShapeDrawer.DrawCircle(field, source, _stepRadius, CellState.Wave);
				allChanges.AddRange(changes);
			}

			foreach (var change in allChanges)
			{
				var point = new Point(change.X, change.Y);
				if (HasBackgroundNeighbor(point, field, CellState.Empty))
				{
					nextSources.Add(point);
				}
				else
				{
					nextSources.Remove(point);
				}
			}
			var nextSourcesArray = nextSources.ToArray();

			return new StepInfo(allChanges.ToArray(), nextSourcesArray);
		}
		private bool HasBackgroundNeighbor(Point point, HuygensField field, CellState backgroundState)
		{
			bool hasBackgroundNeighbor = false;
			for (int i = 0; i < _neighborCoordinates.Length; i++)
			{
				var neighborPoint = new Point(point.X + _neighborCoordinates[i].X, point.Y + _neighborCoordinates[i].Y);

				if (neighborPoint.X < 0 ||
					neighborPoint.Y < 0 ||
					neighborPoint.X >= field.Width ||
					neighborPoint.Y >= field.Height)
				{
					continue;
				}

				if (field[neighborPoint.X, neighborPoint.Y] == backgroundState)
				{
					hasBackgroundNeighbor = true;
					break;
				}
			}

			return hasBackgroundNeighbor;
		}
	}
}
