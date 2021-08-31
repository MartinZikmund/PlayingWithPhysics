using Physics.Shared.UI.Services.ValuesTable;
using Physics.WaveInterference.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.WaveInterference.ValuesTable
{
    public class InterferenceTableRow : ValuesTableRowBase
    {
        private const string TimeFormatting = "0.00";
        private const string DistanceFormatting = "0.0";
		private const string YFormatting = "0.000";

        public string X { get; set; }

		public string First { get; set; }

		public string Second { get; set; }

		public string Interference { get; set; }

        public InterferenceTableRow(float distance, float? first, float? second, float? interference)
        {
			X = distance.ToString(DistanceFormatting);
			First = first?.ToString(YFormatting) ?? Constants.NoValueString;
			Second = second?.ToString(YFormatting) ?? Constants.NoValueString;
			Interference = interference?.ToString(YFormatting) ?? Constants.NoValueString;
		}

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return X;
			yield return First;
			yield return Second;
			yield return Interference;
		}
    }
}
