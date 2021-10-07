using Physics.Shared.UI.Services.ValuesTable;
using System.Collections.Generic;

namespace Physics.LawOfConservationOfMomentum.ValuesTable
{
	public class TableRow : ValuesTableRowBase
    {
        private const string ZeroFormatting = "0.###";

        public string Time { get; set; }

		public string X1 { get; set; }

		public string V1 { get; set; }

		public string X2 { get; set; }

		public string V2 { get; set; }

        public TableRow(float time, float x1, float v1, float x2, float v2)
        {
            Time = time.ToString(ZeroFormatting);
            X1 = x1.ToString(ZeroFormatting);
            V1 = v1.ToString(ZeroFormatting);
            X2 = x2.ToString(ZeroFormatting);
            V2 = v2.ToString(ZeroFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X1;
            yield return V1;
            yield return X2;
            yield return V2;
        }
    }
}
