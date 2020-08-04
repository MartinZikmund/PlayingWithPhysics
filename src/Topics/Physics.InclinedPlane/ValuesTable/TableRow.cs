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
        public string X{ get; set; }
        public string Y { get; set; }
        public string V { get; set; }
        public string S { get; set; }
        public string Ek { get; set; }
        public string Ep { get; set; }
        public string Em { get; set; }
        public string U { get; set; }
        public string E { get; set; }

        public TableRow(float time, float x, float y, float v, float s, float ek, float ep, float em, float u, float e)
        {
            Time = time.ToString(ZeroFormatting);
            X = x.ToString(ZeroFormatting);
            Y = y.ToString(ZeroFormatting);
            V = v.ToString(ZeroFormatting);
            S = s.ToString(ZeroFormatting);
            Ek = ek.ToString(ZeroFormatting);
            Ep = ep.ToString(ZeroFormatting);
            Em = em.ToString(ZeroFormatting);
            U = u.ToString(ZeroFormatting);
            E = e.ToString(ZeroFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X;
            yield return Y;
            yield return V;
            yield return S;
            yield return E;
        }
    }
}
