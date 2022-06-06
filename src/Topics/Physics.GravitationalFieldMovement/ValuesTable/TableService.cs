using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using Physics.GravitationalFieldMovement.Logic;
using Physics.GravitationalFieldMovement.Services;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Services.ValuesTable;

namespace Physics.GravitationalFieldMovement.ValuesTable;

public class TableService : ITableService<TableRow>
{
	private readonly PhysicsService _physicsService;
	private readonly IAppPreferences _appPreferences;
	private readonly double? _time;

	public TableService(PhysicsService physicsService, IAppPreferences appPreferences, double? time)
	{
		_physicsService = physicsService;
		_appPreferences = appPreferences;
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
				var x = _appPreferences.LengthUnit == LengthUnit.Metric ? item.X : MathHelpers.MetersToAstronomicalUnits(item.X);
				var y = _appPreferences.LengthUnit == LengthUnit.Metric ? item.Y : MathHelpers.MetersToAstronomicalUnits(item.Y);
				var h = _appPreferences.LengthUnit == LengthUnit.Metric ? item.H : MathHelpers.MetersToAstronomicalUnits(item.H);
				var r = _appPreferences.LengthUnit == LengthUnit.Metric ? item.R : MathHelpers.MetersToAstronomicalUnits(item.R);
				if (item.H < 0)
				{
					table.Add(new TableRow(item.Time, x, y, item.Phi, MathHelpers.MetersToAstronomicalUnits(_physicsService.Input.Rz), 0, item.V));
					return table;
				}
				table.Add(new TableRow(item.Time, x, y, item.Phi, r, h, item.V));
			}
		}

		return table;
	}
}
