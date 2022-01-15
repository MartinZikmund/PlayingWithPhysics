using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.StationaryWaves.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        private const string TimeFormatting = "0.00";
        private const string DistanceFormatting = "0.00";
		private const string YFormatting = "0.000";

        //public string Time { get; set; }

		[ValuesTableHeader("x")]
		public string X { get; set; }

		[ValuesTableHeader("y₁")]
		public string Y1 { get; set; }

		[ValuesTableHeader("y₂")]
		public string Y2 { get; set; }

		[ValuesTableHeader("y")]
		public string Y { get; set; }

        public TableRow(double x, float? y1, float? y2, float? y)
        {
            X = x.ToString(DistanceFormatting);
			Y1 = y1?.ToString(YFormatting) ?? "";
			Y2 = y2?.ToString(YFormatting) ?? "";
			Y = y?.ToString(YFormatting) ?? "";
		}

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
			yield return X;
			yield return Y1;
			yield return Y2;
			yield return Y;
        }
    }
}
