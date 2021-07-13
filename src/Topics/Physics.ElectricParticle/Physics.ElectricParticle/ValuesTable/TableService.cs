using System;
using System.Collections.Generic;
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
			var setup = _physicsService.Setup;
			bool trajectoryEnded = false;

			List<TableRow> table = new List<TableRow>();
			BigNumber time = 0.0;
			for (int i = 0; i < 10000 /*(ViewModel?.Steps ?? 50)*/; i++)
			{
				BigNumber t = time;
				BigNumber x = _physicsService.ComputeX(time);
				BigNumber y = _physicsService.ComputeY(time);
				BigNumber v = _physicsService.ComputeV(time);
				BigNumber vx = _physicsService.ComputeVx(time);
				BigNumber vy = _physicsService.ComputeVy(time);
				BigNumber a = _physicsService.ComputeA();
				BigNumber ek = _physicsService.ComputeEk(time);
				BigNumber ep = _physicsService.ComputeEp(time);
				BigNumber e = _physicsService.ComputeE(time);

				var yd = (double)y;
				var xd = (double)x;
				var horizontalLimit = setup.HorizontalPlane != null ?
					setup.HorizontalPlane.Distance :
					setup.VerticalPlane.Distance;
				if (Math.Abs((double)yd) > horizontalLimit / 2)
				{
					var maxValue = horizontalLimit / 2;
					y = yd < 0 ? -maxValue : maxValue;
					trajectoryEnded = true;
				}

				var verticalLimit = setup.VerticalPlane != null ?
					setup.VerticalPlane.Distance :
					setup.HorizontalPlane.Distance;

				if (Math.Abs(xd) > verticalLimit / 2)
				{
					var maxValue = verticalLimit / 2;
					x = xd < 0 ? -maxValue : maxValue;
					trajectoryEnded = true;
				}

				var valuesRow = new TableRow(t, x, y, v, vx, vy, a, ek, ep, e);
				table.Add(valuesRow);

				if (trajectoryEnded)
				{
					break;
				}

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
