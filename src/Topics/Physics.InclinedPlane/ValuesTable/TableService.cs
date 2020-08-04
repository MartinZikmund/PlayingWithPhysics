using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.InclinedPlane.ValuesTable
{
    public class TableService : ITableService<TableRow>
    {
        private IPhysicsService _physicsService;

        public TableService(IPhysicsService physicsService)
        {
            _physicsService = physicsService;
        }

        public IEnumerable<TableRow> CalculateTable(float timeInterval)
        {
            List<TableRow> table = new List<TableRow>();
            float cycles = 0;
            float time;
            do
            {
                time = Math.Min(timeInterval * cycles, _physicsService.CalculateMaxT());
                //Add TableRow
                float x = _physicsService.CalculateX(time);
                float y = _physicsService.CalculateY(time);
                float v = _physicsService.CalculateV(time);
                float s = _physicsService.CalculateS(time);
                float ek = _physicsService.CalculateEk(time);
                float ep = _physicsService.CalculateEp(time);
                float em = _physicsService.CalculateEm(time);
                float u = _physicsService.CalculateU(time);
                float e = _physicsService.CalculateE();

                TableRow valuesRow = new TableRow(time, x, y, v, s, ek, ep, em, u, e);
                table.Add(valuesRow);
                cycles++;
            } while (time < _physicsService.CalculateMaxT());

            return table;
        }
    }
}
