using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedNumerics;

namespace Physics.ElectricParticle.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        //private const string ZeroFormatting = "0.###";

        public string Time { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Velocity { get; set; }
        public string VelocityX { get; set; }
        public string VelocityY { get; set; }
        public string Acceleration { get; set; }
        public string Ek { get; set; }
        public string Ep { get; set; }
        public string E { get; set; }

		public TableRow(BigDecimal time, BigDecimal x, BigDecimal y, BigDecimal velocity, BigDecimal velocityX, BigDecimal velocityY, BigDecimal acceleration, BigDecimal ek, BigDecimal ep, BigDecimal e)
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

		//public TableRow(float time, float x, float y, float velocity, float radius, float omega)
  //      {
  //          Time = time.ToString("0.########");
  //          X = x.ToString("0.########");
  //          Y = y.ToString("0.########");
  //          Velocity = velocity.ToString("0.########");
  //          Radius = radius.ToString("0.########");
  //          Omega = omega.ToString("0.########");
  //      }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X;
            yield return Y;
            yield return Velocity;
            yield return VelocityX;
            yield return VelocityY;
            yield return Acceleration;
            yield return Ek;
            yield return Ep;
            yield return E;
        }
    }
}
