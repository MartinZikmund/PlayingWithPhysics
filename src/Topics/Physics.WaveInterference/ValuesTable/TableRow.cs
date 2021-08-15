using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.WaveInterference.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        private const string TimeFormatting = "0.00";
        private const string YFormatting = "0.0000";

        public string Time { get; set; }
		public string X { get; set; }
		public string A { get; set; }

        public TableRow(float time, double x, double a)
        {
            Time = time.ToString(TimeFormatting);
            X = x.ToString(YFormatting);
			A = a.ToString(YFormatting);
		}

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
			yield return X;
			yield return A;
        }
    }
}
