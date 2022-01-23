using System;
using System.Collections.Generic;
using Physics.CyclicProcesses.Logic.Physics;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.CyclicProcesses.ValuesTable
{
	public class BasicProcessTableService : ITableService<BasicProcessTableRow>
	{
		private readonly IBasicProcessPhysicsService _physicsService;

		public BasicProcessTableService(IBasicProcessPhysicsService physicsService)
		{
			_physicsService = physicsService ?? throw new ArgumentNullException(nameof(physicsService));
		}

		public IEnumerable<BasicProcessTableRow> CalculateTable(float timeInterval)
		{
			var table = new List<BasicProcessTableRow>();

			float minTime = 0;
			float maxTime = 10;
			var time = minTime;
			while (time <= maxTime)
			{
				var v = _physicsService.CalculateV(time);
				var p = _physicsService.CalculateP(time);
				var t = _physicsService.CalculateT(time);
				var w = _physicsService.CalculateW(time);
				var q = _physicsService.CalculateQ(time);
				var deltaU = _physicsService.CalculateDeltaU(time);
				var row = new BasicProcessTableRow(v, p, t, w, q, deltaU);
				table.Add(row);
				if (time == maxTime)
				{
					break;
				}
				time += timeInterval;
				time = Math.Min(time, maxTime);
			}

			return table;
		}
	}
}
