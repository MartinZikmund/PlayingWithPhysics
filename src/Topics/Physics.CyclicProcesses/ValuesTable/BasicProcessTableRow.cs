using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.CyclicProcesses.ValuesTable
{
	public class BasicProcessTableRow : ValuesTableRowBase
	{
		private const string TimeFormatting = "0.00";
		private const string DistanceFormatting = "0.00";
		private const string YFormatting = "0.000";

		public BasicProcessTableRow(double v, float p, float t, float w, float q, float deltaU)
		{
			V = v.ToString(DistanceFormatting);
			P = p.ToString(YFormatting) ?? "";
			T = t.ToString(YFormatting) ?? "";
			W = w.ToString(YFormatting) ?? "";
			Q = q.ToString(YFormatting) ?? "";
			DeltaU = deltaU.ToString(YFormatting) ?? "";
		}

		[ValuesTableHeader("V (m³)")]
		public string V { get; set; }

		[ValuesTableHeader("p (Pa)")]
		public string P { get; set; }

		[ValuesTableHeader("T (K)")]
		public string T { get; set; }

		[ValuesTableHeader("W (J)")]
		public string W { get; set; }

		[ValuesTableHeader("Q (J)")]
		public string Q { get; set; }

		[ValuesTableHeader("ΔU (J)")]
		public string DeltaU { get; set; }
	}
}
