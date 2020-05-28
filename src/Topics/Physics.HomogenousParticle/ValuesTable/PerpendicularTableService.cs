using Physics.HomongenousParticle.Logic.PhysicsServices;
using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.HomogenousParticle.ValuesTable
{
    public class PerpendicularTableService : ITableService<PerpendicularTableRow>
    {
        private PerpendicularPhysicsService _physicsService;

        public PerpendicularTableService(PerpendicularPhysicsService physicsService)
        {
            _physicsService = physicsService;
        }

        public IEnumerable<PerpendicularTableRow> CalculateTable(float timeInterval)
        {
            List<PerpendicularTableRow> table = new List<PerpendicularTableRow>();
            var time = 0.0;
            float cycles = 0;
            do
            {
                time = (double)Math.Min(timeInterval * cycles, _physicsService.MaxT);
                double x = _physicsService.ComputeX(time);
                double y = _physicsService.ComputeY(time);
                double omega = (double)_physicsService.ComputeOmega();
                double radius = (double)_physicsService.ComputeRadius();
                double velocity = (double)_physicsService.ComputeVelocity();

                var valuesRow = new PerpendicularTableRow((float)time, (float)x, (float)y, (float)velocity, (float)radius, (float)omega);
                table.Add(valuesRow);

                cycles++;
            } while (time < _physicsService.MaxT);

            return table;
        }
    }
}
