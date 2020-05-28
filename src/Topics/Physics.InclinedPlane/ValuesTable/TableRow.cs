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
        private const string ZeroFormatting = "0.###";

        public string Time { get; set; }
        public string V { get; set; }
        public string S { get; set; }
        public string Ev { get; set; }

        public TableRow(float time, float v, float s/*, float ev*/)
        {
            Time = time.ToString(ZeroFormatting);
            V = v.ToString(ZeroFormatting);
            S = s.ToString(ZeroFormatting);
            //Ev = ev.ToString(ZeroFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return V;
            yield return S;
            yield return Ev;
        }
    }
}
