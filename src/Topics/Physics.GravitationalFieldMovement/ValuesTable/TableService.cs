using System;
using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.GravitationalFieldMovement.Logic;

namespace Physics.GravitationalFieldMovement.ValuesTable;

public class TableService : ITableService<TableRow>
{
	private readonly PhysicsService _physicsService;
	private readonly double? _time;
	public TableService(PhysicsService physicsService, double? time)
	{
		_physicsService = physicsService;
		_time = time;
	}

	public ValuesTableDialogViewModel Owner { get; set; }

	public IEnumerable<TableRow> CalculateTable(float timeInterval)
	{
		if (Owner == null)
		{
			return Array.Empty<TableRow>();
		}

		var trajectory = _physicsService.CalculateTrajectory();

		List<TableRow> table = new List<TableRow>();

		foreach (var item in trajectory)
		{
			if (_time == null || item.Time <= _time)
			{
				if (item.H < 0)
				{
					table.Add(new TableRow(item.Time, item.X, item.Y, item.V, 0));
					return table;
				}
				table.Add(new TableRow(item.Time, item.X, item.Y, item.V, item.H));
			}
		}

		return table;
	}
}
