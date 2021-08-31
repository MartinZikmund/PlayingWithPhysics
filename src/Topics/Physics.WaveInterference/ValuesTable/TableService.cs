using System.Collections.Generic;
using Physics.WaveInterference.Logic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.WaveInterference.ValuesTable
{
	public class TableService : ITableService<TableRow>
	{
		private const int MaxCycles = 500;
		private const int MaxTimeInSeconds = 120;

		private IWavePhysicsService _physicsService;
		public bool Compound { get; }

		public TableService(IWavePhysicsService physicsService, bool compound)
		{
			_physicsService = physicsService;
			Compound = compound;
		}

		public IEnumerable<TableRow> CalculateTable(float timeInterval)
		{
			List<TableRow> table = new List<TableRow>();
			float cycles = 0;
			float time = 0;
			float x = -20f;

			while (x <= 20f)
			{
				var a = _physicsService.CalculateA(x, time);
				TableRow valuesRow = new TableRow(0f, x, a);
				table.Add(valuesRow);
				if (Compound)
				{
					x += ((WaveInterferencePhysicsService)_physicsService).Delta;
				}
				else
				{
					x += ((WavePhysicsService)_physicsService).Delta;
				}
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
