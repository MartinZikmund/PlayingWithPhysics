namespace Physics.HuygensPrinciple.Logic
{
	public interface IShape
	{
		void Draw(HuygensField field);

		public CellState State { get; }

		bool HitTest(double relativeX, double relativeY);
	}
}
