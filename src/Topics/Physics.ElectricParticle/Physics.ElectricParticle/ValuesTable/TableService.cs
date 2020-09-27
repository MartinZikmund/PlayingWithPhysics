using Physics.ElectricParticle.Logic;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.ValuesTable
{
    public class TableService : ITableService<TableRow>
    {
        private PhysicsService _physicsService;

        public TableService(PhysicsService physicsService)
        {
            _physicsService = physicsService;
        }

        public IEnumerable<TableRow> CalculateTable(float timeInterval)
        {
            List<TableRow> table = new List<TableRow>();
            var time = 0.0;
            float cycles = 0;
            do
            {
                time = (double)Math.Min(timeInterval * cycles, _physicsService.MaxT);
                double x = _physicsService.ComputeX(time);
                double y = _physicsService.ComputeY(time);
                //double omega = (double)_physicsService.ComputeOmega();
                //double radius = (double)_physicsService.ComputeRadius();
                //double velocity = (double)_physicsService.ComputeVelocity();

                // var valuesRow = new TableRow((float)time, (float)x, (float)y, (float)velocity, (float)radius, (float)omega);
                var valuesRow = new TableRow(0f,0f,0f,0f,0f,0f);
                table.Add(valuesRow);

                cycles++;
            } while (time < _physicsService.MaxT);

            return table;
        }
    }
}
