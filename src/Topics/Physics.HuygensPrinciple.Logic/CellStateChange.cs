namespace Physics.HuygensPrinciple.Logic
{
	public struct CellStateChange
	{
		public CellStateChange(int x, int y, CellState newState)
		{
			X = x;
			Y = y;
			NewState = newState;
		}

		public CellState NewState { get; set; }

		public int X { get; set; }

		public int Y { get; set; }
	}
}
