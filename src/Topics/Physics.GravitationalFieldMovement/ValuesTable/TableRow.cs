using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.GravitationalFieldMovement.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        private const string DistanceFormatting = "0.00";
		private const string TFormatting = "0.00";

		[ValuesTableHeader("t (s)")]
		public string T { get; set; }

		[ValuesTableHeader("x (m)")]
		public string X { get; set; }

		[ValuesTableHeader("y (m)")]
		public string Y { get; set; }

		[ValuesTableHeader("v (m.s⁻¹)")]
		public string V { get; set; }

		[ValuesTableHeader("h (m)")]
		public string H { get; set; }

		public TableRow(double t, double x, double y, double v, double h)
        {
			T = t.ToString(TFormatting);
            X = x.ToString(DistanceFormatting);
			Y = y.ToString(DistanceFormatting) ?? "";
			V = v.ToString(DistanceFormatting) ?? "";
			H = h.ToString(DistanceFormatting) ?? "";
		}

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
			yield return T;
			yield return X;
			yield return Y;
			yield return V;
			yield return H;
        }
    }
}
