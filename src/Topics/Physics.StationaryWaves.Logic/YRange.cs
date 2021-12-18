namespace Physics.StationaryWaves.Logic
{
	public struct YRange
	{
		public YRange(float min, float max)
		{
			MinY = min;
			MaxY = max;
		}

		public float MinY { get; }

		public float MaxY { get; }
	}
}
