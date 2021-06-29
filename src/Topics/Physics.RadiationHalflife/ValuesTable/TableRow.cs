using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.RadiationHalflife.ValuesTable
{
	public class TableRow : ValuesTableRowBase
	{
		private const string TimeFormatting = "0.00";
		private const string YFormatting = "0.00";

		public string Time { get; set; }

		public string OtherValue { get; set; }

		public TableRow(float time, double otherValue)
		{
			Time = time.ToString(TimeFormatting);
			OtherValue = otherValue.ToString(YFormatting);
			
		}

		protected override IEnumerable<string> GetCellValuesInOrder()
		{
			yield return Time;
			yield return OtherValue;
		}
	}
}
