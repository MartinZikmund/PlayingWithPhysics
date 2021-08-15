using System.Collections.Generic;
using Physics.WaveInterference.Logic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.WaveInterference.ValuesTable
{
	public class TableService : ITableService<TableRow>
	{
		private const int MaxCycles = 500;
		private const int MaxTimeInSeconds = 120;

		private WavePhysicsService _physicsService;

		public TableService(WavePhysicsService physicsService)
		{
			_physicsService = physicsService;
		}

		public IEnumerable<TableRow> CalculateTable(float timeInterval)
		{
			List<TableRow> table = new List<TableRow>();
			float cycles = 0;
			float time;
			float x = -20f;

			while (x <= 20f)
			{
				var a = _physicsService.CalculateA(x);
				TableRow valuesRow = new TableRow(0f, x, a);
				table.Add(valuesRow);

				x += _physicsService.Delta;
			}

			//do
			//{
			//	time = timeInterval * cycles;
			//	//Add TableRow
			//	var a = _physicsService.CalculateA(time);

			//	TableRow valuesRow = new TableRow(time, x, a);
			//	table.Add(valuesRow);
			//	cycles++;
			//} while (cycles < MaxCycles || time < MaxTimeInSeconds);

			return table;
		}
	}
}
