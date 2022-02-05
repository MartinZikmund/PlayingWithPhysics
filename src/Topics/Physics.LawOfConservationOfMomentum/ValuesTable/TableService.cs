using System.Collections.Generic;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.LawOfConservationOfMomentum.ValuesTable
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
			float time = 0f;
			do
			{
				//Add TableRow
				float x1 = _physicsService.GetX1(time);
				float v1 = _physicsService.GetV1(time);
				float x2 = _physicsService.GetX2(time);
				float v2 = _physicsService.GetV2(time);

				TableRow valuesRow = new TableRow(time, x1, v1, x2, v2);
				table.Add(valuesRow);
				time = time += timeInterval;
			} while (time <= 60);

			return table;
		}
	}
}
