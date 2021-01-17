using System.Collections.Generic;
using Physics.LissajousCurves.Logic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.LissajousCurves.ValuesTable
{
	public class TableService : ITableService<TableRow>
	{
		private const int MaxCycles = 500;
		private const int MaxTimeInSeconds = 120;

		private IOscillationPhysicsService _physicsService;

		public TableService(IOscillationPhysicsService physicsService)
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
				time = timeInterval * cycles;
				//Add TableRow
				var y = _physicsService.CalculateY(time);
				var v = _physicsService.CalculateV(time);
				var a = _physicsService.CalculateA(time);

				TableRow valuesRow = new TableRow(time, y, v, a);
				table.Add(valuesRow);
				cycles++;
			} while (cycles < MaxCycles || time < MaxTimeInSeconds);

			return table;
		}
	}
}
