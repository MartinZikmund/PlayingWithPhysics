using System.Drawing;

namespace Physics.HuygensPrinciple.Logic
{
	public class StepInfo
    {
		public StepInfo(CellStateChange[] cellStateChanges, Point[] waveBorder)
		{
			CellStateChanges = cellStateChanges;
			WaveBorder = waveBorder;
		}

        public CellStateChange[] CellStateChanges { get; }

		public Point[] WaveBorder { get; }
    }
}
