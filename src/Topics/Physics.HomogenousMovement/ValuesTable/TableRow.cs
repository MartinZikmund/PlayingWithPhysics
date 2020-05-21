using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.HomogenousMovement
{
    public class TableRow : ValuesTableRowBase
    {
        private const string ZeroFormatting = "0.###";

        public string Time { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string V { get; set; }
        public string VX { get; set; }
        public string VY { get; set; }
        public string EP { get; set; }
        public string EK { get; set; }
        public string EPEK { get; set; }

        public TableRow(float time, float x, float y, float v, float vx, float vy, float ep, float ek, float epek)
        {
            Time = time.ToString(ZeroFormatting);
            X = x.ToString(ZeroFormatting);
            Y = y.ToString(ZeroFormatting);
            V = v.ToString(ZeroFormatting);
            VX = vx.ToString(ZeroFormatting);
            VY = vy.ToString(ZeroFormatting);
            EP = ep.ToString(ZeroFormatting);
            EK = ek.ToString(ZeroFormatting);
            EPEK = epek.ToString(ZeroFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X;
            yield return Y;
            yield return V;
            yield return VX;
            yield return VY;
            yield return EP;
            yield return EK;
            yield return EPEK;
        }
    }
}
