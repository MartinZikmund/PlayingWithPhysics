using Physics.Shared.UI.Services.ValuesTable;
using System.Collections.Generic;

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

		public string W { get; set; }

		public string Ep { get; set; }

		public string Ek { get; set; }

		public string Er { get; set; }

        public TableRow(float time, float x, float y, float v, float s, float w, float ek, float ep, float er)
        {
            Time = time.ToString(ZeroFormatting);
            X = x.ToString(ZeroFormatting);
            Y = y.ToString(ZeroFormatting);
            V = v.ToString(ZeroFormatting);
            S = s.ToString(ZeroFormatting);
			W = w.ToString(ZeroFormatting);
            Ek = ek.ToString(ZeroFormatting);
            Ep = ep.ToString(ZeroFormatting);
            Er = er.ToString(ZeroFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X;
            yield return Y;
            yield return V;
            yield return S;
			yield return W;
            yield return Ek;
            yield return Ep;
            yield return Er;
        }
    }
}
