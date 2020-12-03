using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        private const string TimeFormatting = "0.00";
        private const string YFormatting = "0.0000";

        public string Time { get; set; }

		public string Y { get; set; }

        public TableRow(float time, double y)
        {
            Time = time.ToString(TimeFormatting);
            Y = y.ToString(YFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return Y;
        }
    }
}
