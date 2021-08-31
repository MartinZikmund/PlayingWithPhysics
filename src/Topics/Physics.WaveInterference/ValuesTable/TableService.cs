using System;
using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.WaveInterference.Logic;

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

			float x = -20f;

			while (x <= 20f)
			{
				var a = _physicsService.CalculateY(x, time);
				TableRow valuesRow = new TableRow(x, a);
				table.Add(valuesRow);
				x += distanceInterval;
			}

			return table;
		}
	}
}
