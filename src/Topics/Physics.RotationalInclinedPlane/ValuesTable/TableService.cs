using Physics.RotationalInclinedPlane.Logic;
using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;

namespace Physics.InclinedPlane.ValuesTable
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
            float cycles = 0;
            float time;
            do
            {
                time = Math.Min(timeInterval * cycles, _physicsService.CalculateMaxT());
                //Add TableRow
                float x = _physicsService.CalculateX(time);
                float y = _physicsService.CalculateY(time);
                float v = _physicsService.CalculateVelocity(time);
                float s = _physicsService.CalculateDistance(time);
                float w = _physicsService.CalculateAngularVelocity(time);
				float ek = _physicsService.CalculateEk(time);
                float ep = _physicsService.CalculateEp(time);
                float er = _physicsService.CalculateEr(time);

                TableRow valuesRow = new TableRow(time, x, y, v, s, w, ek, ep, er);
                table.Add(valuesRow);
                cycles++;
            } while (time < _physicsService.CalculateMaxT());

            return table;
        }
    }
}
