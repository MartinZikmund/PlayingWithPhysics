using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics.Shared.Mathematics;

namespace Physics.GravitationalFieldMovement.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
		private const int TableSignificantDigits = 5;

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
			T = t.ToSignificantDigitsString(TableSignificantDigits);
			X = x.ToSignificantDigitsString(TableSignificantDigits);
			Y = y.ToSignificantDigitsString(TableSignificantDigits);
			V = v.ToSignificantDigitsString(TableSignificantDigits);
			H = h.ToSignificantDigitsString(TableSignificantDigits);
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
