namespace Physics.HuygensPrinciple.Logic
{
	public class StepInfo
    {
		public StepInfo(CellStateChange[] cellStateChanges)
		{
			CellStateChanges = cellStateChanges;
		}

        public CellStateChange[] CellStateChanges { get; }
    }
}
