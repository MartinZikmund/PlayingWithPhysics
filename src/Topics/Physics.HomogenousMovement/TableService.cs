using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.UI;
using Physics.HomogenousMovement.PhysicsServices;

namespace Physics.HomogenousMovement
{
    public class TableService
    {
        private IPhysicsService _physicsService;

        public List<TableRow> PopulateTable(float timeInterval)
        {
            List<TableRow> table = new List<TableRow>();
            float currentY = _physicsService.ThrowInfo.Origin.Y;
            float cycles = 0;
            float time = 0.0f;
            do
            {
                time = Math.Min(timeInterval * cycles, _physicsService.MaxT);
                //Add TableRow
                float x = _physicsService.ComputeX(time);
                float y = _physicsService.ComputeY(time);
                float v = _physicsService.ComputeV(time);
                float vx = _physicsService.ComputeVX(time);
                float vy = _physicsService.ComputeVY(time);
                float ep = _physicsService.ComputeEP(time);
                float ek = _physicsService.ComputeEK(time);
                float epek = _physicsService.ComputeEPEK(time);

                TableRow valuesRow = new TableRow(time, x, y, v, vx, vy, ep, ek, epek);
                table.Add(valuesRow);

                currentY = y;
                cycles++;
            } while (time < _physicsService.MaxT);

            return table;

        }

        public TableService(IPhysicsService physicsService)
        {
            _physicsService = physicsService;
        }
    }

    public class TableRow
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
    }
}
