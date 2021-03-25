﻿using System.Collections.Generic;
using ExtendedNumerics;
using Physics.ElectricParticle.Logic;
using Physics.Shared.Mathematics;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.ElectricParticle.ValuesTable
{
	public class TableService : ITableService<TableRow>
	{
		private PhysicsService _physicsService;

		public TableService(PhysicsService physicsService)
		{
			_physicsService = physicsService;
		}

		public ValuesTableDialogViewModel ViewModel { get; internal set; }

		public IEnumerable<TableRow> CalculateTable(float timeInterval)
		{
			var deltaT = _physicsService.ComputeDeltaT(ViewModel?.Steps ?? 50);

			List<TableRow> table = new List<TableRow>();
			BigNumber time = 0.0;
			for (int i = 0; i < (ViewModel?.Steps ?? 50); i++)
			{
				BigNumber t = _physicsService.ComputeX(time);
				BigNumber x = _physicsService.ComputeX(time);
				BigNumber y = _physicsService.ComputeY(time);
				BigNumber v = _physicsService.ComputeY(time);
				BigNumber vx = _physicsService.ComputeY(time);
				BigNumber vy = _physicsService.ComputeY(time);
				BigNumber a = _physicsService.ComputeY(time);
				BigNumber ek = _physicsService.ComputeY(time);
				BigNumber ep = _physicsService.ComputeY(time);
				BigNumber e = _physicsService.ComputeY(time);

				var valuesRow = new TableRow(t, x, y, v, vx, vy, a, ep, ek, e);

				table.Add(valuesRow);

				time += deltaT;
			}

			return table;
		}

		private BigDecimal Min(BigDecimal leftDecimal, BigDecimal rightDecimal)
		{
			if (leftDecimal < rightDecimal)
				return leftDecimal;
			else if (leftDecimal > rightDecimal)
				return rightDecimal;
			else
				return leftDecimal;
		}
	}
}
