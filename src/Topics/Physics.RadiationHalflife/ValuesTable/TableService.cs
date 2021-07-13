using System.Collections.Generic;
using Physics.RadiationHalflife.Logic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.RadiationHalflife.ValuesTable
{
	public class TableService : ITableService<TableRow>
	{
		private const int MaxCycles = 500;
		private const int MaxTimeInSeconds = 120;

		private PhysicsService _physicsService;
		private PhenomenonVariant _variant;

		public TableService(PhenomenonVariant variant, PhysicsService physicsService)
		{
			_variant = variant;
			if (variant == PhenomenonVariant.RadioactiveLaw)
			{
				_physicsService = (RadioactiveLawPhysicsService)physicsService;
			}
			else
			{
				_physicsService = (BeamActivityPhysicsService)physicsService;
			}
		}

		public IEnumerable<TableRow> CalculateTable(float timeInterval)
		{
			List<TableRow> table = new List<TableRow>();
			float cycles = 0;
			float time;

			if (_variant == PhenomenonVariant.RadioactiveLaw)
			{
				var service = ((RadioactiveLawPhysicsService)_physicsService);
				service.FillTable();
				List<(float value1, int value2)> originalTable = service.ValuesTable;
				foreach(var item in originalTable)
				{
					TableRow row = new TableRow(item.value1, item.value2);
					table.Add(row);
				}
			}
			else
			{
				var service = ((BeamActivityPhysicsService)_physicsService);
				List<(float value1, float value2)> originalTable = service.FillTable();
				foreach (var item in originalTable)
				{
					TableRow row = new TableRow(item.value1, item.value2);
					table.Add(row);
				}
			}

			//do
			//{
			//	time = timeInterval * cycles;
			//	Add TableRow
			//	float otherValue = 0;
			//	if (_variant == PhenomenonVariant.RadioactiveLaw)
			//	{
			//		otherValue = ((RadioactiveLawPhysicsService)_physicsService).ComputeN(time);
			//	}
			//	else
			//	{
			//		otherValue = ((BeamActivityPhysicsService)_physicsService).ComputeA(time);
			//	}

			//	TableRow valuesRow = new TableRow(time, otherValue);
			//	table.Add(valuesRow);
			//	cycles++;
			//} while (cycles < MaxCycles || time < MaxTimeInSeconds);

			return table;
		}
	}
}
