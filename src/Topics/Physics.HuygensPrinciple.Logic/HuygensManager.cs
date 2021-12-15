using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Physics.HuygensPrinciple.Logic
{
	public class HuygensManager
	{
		private readonly HuygensStepper _stepper = null;

		private readonly HuygensField _originalField;
		private HuygensField _currentField;
		private int _fieldWidth;
		private int _fieldHeight;

		public HuygensManager(HuygensField originalField)
		{
			_originalField = originalField;
			_currentField = _originalField.Clone();
			_fieldHeight = _originalField.Height;
			_fieldWidth = _originalField.Width;
			_stepper = new HuygensStepper(originalField, 10.5f);
		}

		public HuygensField OriginalField => _originalField;

		public HuygensField CurrentField => _currentField;

		public IList<Point> GetBorderPoints(HuygensField field, CellState spotState, CellState backgroundState = 0) =>
			_stepper.GetBorderPoints(field, spotState, backgroundState);

		public async Task PrecalculateAsync() => await _stepper.PrecalculateStepsAsync();

		public CellStateChange[] NextStep()
		{
			if (CurrentStep >= _stepper.StepsAvailable)
			{
				return Array.Empty<CellStateChange>();
			}
			var step = _stepper.GetStep(CurrentStep);
			CurrentStep++;
			return step.CellStateChanges;
			//ResetField();

			//for (int i = 0; i < step; i++)
			//{
			//	NextStep();
			//}
		}

		public int CurrentStep { get; set; } = 0;

		public int FieldHeight => _fieldHeight;

		public int FieldWidth => _fieldWidth;
	}
}
