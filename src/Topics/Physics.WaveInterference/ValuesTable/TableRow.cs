using Physics.Shared.UI.Services.ValuesTable;
using Physics.WaveInterference.Logic;
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
        private const string DistanceFormatting = "0.0";
		private const string YFormatting = "0.000";

        //public string Time { get; set; }

		public string X { get; set; }

		public string Y { get; set; }

        public TableRow(double x, float? y)
        {
            X = x.ToString(DistanceFormatting);
			Y = y?.ToString(YFormatting) ?? Constants.NoValueString;
		}

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            //yield return Time;
			yield return X;
			yield return Y;
        }
    }
}
