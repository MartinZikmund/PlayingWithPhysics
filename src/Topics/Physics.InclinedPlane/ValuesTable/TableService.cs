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
            float time = 0.0f;
            do
            {
                time = Math.Min(timeInterval * cycles, _physicsService.MaxT);
                //Add TableRow
                float x = _physicsService.ComputeX(time);
                float y = _physicsService.ComputeY(time);
                float v = _physicsService.ComputeV(time);
                float s = _physicsService.ComputeS(time);
                float ev = _physicsService.ComputeEv(time);

                TableRow valuesRow = new TableRow(time, x, y, v, s, ev);
                table.Add(valuesRow);
                cycles++;
            } while (time < _physicsService.MaxT);

            return table;
        }
    }
}
