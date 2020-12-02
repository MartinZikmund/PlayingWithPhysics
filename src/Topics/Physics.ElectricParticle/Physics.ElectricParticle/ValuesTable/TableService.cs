using Physics.ElectricParticle.Logic;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedNumerics;

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
            BigDecimal time = 0.0;
            float cycles = 0;
            do
            {
                time = Min(timeInterval * cycles, _physicsService.MaxT);
				//BigDecimal t = _physicsService.ComputeX(time);
				//BigDecimal x = _physicsService.ComputeX(time);
				//BigDecimal y = _physicsService.ComputeY(time);
				//BigDecimal v = _physicsService.ComputeY(time);
				//BigDecimal vx = _physicsService.ComputeY(time);
				//BigDecimal vy = _physicsService.ComputeY(time);
				//BigDecimal a = _physicsService.ComputeY(time);
				//BigDecimal ek = _physicsService.ComputeY(time);
				//BigDecimal ep = _physicsService.ComputeY(time);
				//BigDecimal e = _physicsService.ComputeY(time);

				//var valuesRo	 w = new TableRow(t, x,y, v, vx, vy, a, ep, ek, e);
                table.Add(null);

                cycles++;
            } while (time < _physicsService.MaxT);

            return table;
        }

		private BigDecimal Min(BigDecimal leftDecimal, BigDecimal rightDecimal)
		{
			if (leftDecimal < rightDecimal)
				return leftDecimal;
			else if (leftDecimal > rightDecimal)
				return rightDecimal;
			else
				return leftDecimal;
		}
    }
}
