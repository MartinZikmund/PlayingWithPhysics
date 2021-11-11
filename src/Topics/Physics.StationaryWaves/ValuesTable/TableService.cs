using System;
using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.StationaryWaves.Logic;

namespace Physics.StationaryWaves.ValuesTable
{
	public class TableService : ITableService<TableRow>
	{
		private IWavePhysicsService _physicsService;

		public bool Compound { get; }

		public TableService(IWavePhysicsService physicsService, bool compound)
		{
			_physicsService = physicsService;
			Compound = compound;
		}

		public ValuesTableDialogViewModel Owner { get; set; }

		public IEnumerable<TableRow> CalculateTable(float timeInterval)
		{
			if (Owner == null)
			{
				return Array.Empty<TableRow>();
			}

			var time = Owner.Time;
			var distanceInterval = Owner.DistanceInterval;

			List<TableRow> table = new List<TableRow>();

			float minX = 0;
			float maxX = 6 * (float)Math.PI;
			var x = minX;
			while (x <= maxX)
			{
				var y1 = _physicsService.CalculateFirstWaveY(x, time);
				var y2 = _physicsService.CalculateSecondWaveY(x, time);
				var y = y1 + y2;
				TableRow valuesRow = new TableRow(x, y1, y2, y);
				table.Add(valuesRow);
				if (x == maxX)
				{
					break;
				}
				x += distanceInterval;
				x = Math.Min(x, maxX);
			}

			return table;
		}
	}
}
