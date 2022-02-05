using System;
using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.GravitationalFieldMovement.Logic;

namespace Physics.GravitationalFieldMovement.ValuesTable;

public class TableService : ITableService<TableRow>
{
	private readonly PhysicsService _physicsService;

	public TableService(PhysicsService physicsService)
	{
		_physicsService = physicsService;
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
			table.Add(new TableRow(item.Time, item.X, item.Y, item.V));
		}

		return table;
	}
}
