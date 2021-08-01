using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.HomogenousMovement
{
    public class TableRow : ValuesTableRowBase
    {
        private const string ZeroFormatting = "0.###";

		[ValuesTableHeader("t (s)")]
        public string Time { get; set; }

		[ValuesTableHeader("x (m)")]
        public string X { get; set; }

		[ValuesTableHeader("y (m)")]
        public string Y { get; set; }

		[ValuesTableHeader("v (m/s)")]
		public string V { get; set; }

		[ValuesTableHeader("vx (m/s)")]
        public string VX { get; set; }

		[ValuesTableHeader("vy (m/s)")]
		public string VY { get; set; }

		[ValuesTableHeader("Ep (J)")]
        public string EP { get; set; }

		[ValuesTableHeader("Ek (J)")]
		public string EK { get; set; }

		[ValuesTableHeader("Ep + Ek (J)")]
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
    }
}
