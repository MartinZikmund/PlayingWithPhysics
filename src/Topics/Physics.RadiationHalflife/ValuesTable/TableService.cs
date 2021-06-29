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
			do
			{
				time = timeInterval * cycles;
				//Add TableRow
				float otherValue = 0;
				if (_variant == PhenomenonVariant.RadioactiveLaw)
				{
					otherValue = ((RadioactiveLawPhysicsService)_physicsService).ComputeN(time);
				}
				else
				{
					otherValue = ((BeamActivityPhysicsService)_physicsService).ComputeA(time);
				}

				TableRow valuesRow = new TableRow(time, otherValue);
				table.Add(valuesRow);
				cycles++;
			} while (cycles < MaxCycles || time < MaxTimeInSeconds);

			return table;
		}
	}
}
