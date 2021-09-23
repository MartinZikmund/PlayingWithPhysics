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

			float minX = Math.Max(_physicsService.StartX, _physicsService.Wave.OriginX - _physicsService.Wave.WaveLength * 3);
			float maxX = Math.Min(_physicsService.EndX, _physicsService.Wave.OriginX + _physicsService.Wave.WaveLength * 3);
			var x = minX;
			while (x <= maxX)
			{
				var a = _physicsService.CalculateY(x, time);
				TableRow valuesRow = new TableRow(x, a);
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
