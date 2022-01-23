namespace Physics.CyclicProcesses.Logic.Input
{
	public class StirlingEngineInputConfiguration : IInputConfiguration
	{
		public StirlingEngineInputConfiguration(
			float n,
			float v1,
			float v2,
			float t12,
			float t34)
		{
			N = n;
			V1 = v1;
			V2 = v2;
			T12 = t12;
			T34 = t34;
		}

		/// <summary>
		/// Input variant.
		/// </summary>
		public ProcessType Process => ProcessType.StirlingEngine;

		/// <summary>
		/// Lower volume.
		/// </summary>
		public float V1 { get; }

		/// <summary>
		/// Upper volume.
		/// </summary>
		public float V2 { get; }

		/// <summary>
		/// Cooler temperature.
		/// </summary>
		public float T12 { get; }

		/// <summary>
		/// Heater temperature.
		/// </summary>
		public float T34 { get; }

		/// <summary>
		/// Number of mols.
		/// </summary>
		public float N { get; set; }
	}
}
