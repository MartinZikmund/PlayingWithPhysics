using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.HomogenousParticle.ValuesTable
{
    public class PerpendicularTableRow : ValuesTableRowBase
    {
        private const string ZeroFormatting = "0.###";

        public string Time { get; set; }

        public string X { get; set; }

        public string Y { get; set; }

        public string Omega { get; set; }

        public string Velocity { get; set; }

        public string Radius { get; set; }

        public PerpendicularTableRow(float time, float x, float y, float velocity, float radius, float omega)
        {            
            Time = time.ToString("0.########");
            X = x.ToString("0.########");
            Y = y.ToString("0.########");
            Velocity = velocity.ToString("0.########");
            Radius = radius.ToString("0.########");
            Omega = omega.ToString("0.########");
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X;
            yield return Y;
            yield return Velocity;
            yield return Radius;
            yield return Omega;
        }
    }
}
