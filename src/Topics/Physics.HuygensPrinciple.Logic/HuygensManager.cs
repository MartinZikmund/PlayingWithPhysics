using System;
using System.Collections;
using System.Collections.Generic;
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
			_stepper = new HuygensStepper(originalField, 4);
		}

		public HuygensField CurrentField => _currentField;

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
