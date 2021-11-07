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
		}

		public void SetStep(int step)
		{
			ResetField();

			for (int i = 0; i < step; i++)
			{
				NextStep();
			}
		}

		public int CurrentStep { get; set; } = 0;

		public int FieldHeight => _fieldHeight;

		public int FieldWidth => _fieldWidth;

		//private void PerformCachedStep(int step)
		//{
		//	if (step >= _stepsCache.Count)
		//	{
		//		throw new InvalidOperationException("This step is not cached yet.");
		//	}

		//	var stepInfo = _stepsCache[step];
		//	foreach (var cellStateChange in stepInfo.CellStateChanges)
		//	{
		//		_currentField[cellStateChange.X, cellStateChange.Y] = cellStateChange.NewState;
		//	}
		//}
	}
}
