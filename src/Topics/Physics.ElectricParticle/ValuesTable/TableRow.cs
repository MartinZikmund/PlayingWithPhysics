using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedNumerics;
using Physics.Shared.Mathematics;

namespace Physics.ElectricParticle.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        [ValuesTableHeader("t (s)")]
        public string Time { get; set; }

		[ValuesTableHeader("x (m)")]
        public string X { get; set; }

		[ValuesTableHeader("y (m)")]
        public string Y { get; set; }

		[ValuesTableHeader("v (m/s)")]
        public string Velocity { get; set; }

		[ValuesTableHeader("vx (m/s)")]
        public string VelocityX { get; set; }

		[ValuesTableHeader("vy (m/s)")]
		public string VelocityY { get; set; }

		[ValuesTableHeader("a (m/s²)")]
		public string Acceleration { get; set; }

		[ValuesTableHeader("Ek (J)")]
        public string Ek { get; set; }

		[ValuesTableHeader("Ep (J)")]
		public string Ep { get; set; }

		[ValuesTableHeader("E (J)")]
		public string E { get; set; }

		public TableRow(
			BigNumber time,
			BigNumber x,
			BigNumber y,
			BigNumber velocity,
			BigNumber velocityX,
			BigNumber velocityY,
			BigNumber acceleration,
			BigNumber ek,
			BigNumber ep,
			BigNumber e)
		{
			Time = time.ToString();
			X = x.ToString();
			Y = y.ToString();
			Velocity = velocity.ToString();
			VelocityX = velocityX.ToString();
			VelocityY = velocityY.ToString();
			Acceleration = acceleration.ToString();
			Ek = ek.ToString();
			Ep = ep.ToString();
			E = e.ToString();
		}
    }
}
