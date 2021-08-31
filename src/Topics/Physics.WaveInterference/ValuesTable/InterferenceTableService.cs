using System;
using System.Collections.Generic;
using Physics.Shared.UI.Services.ValuesTable;
using Physics.WaveInterference.Logic;

namespace Physics.WaveInterference.ValuesTable
{
	public class InterferenceTableService : ITableService<InterferenceTableRow>
	{
		private const int MaxCycles = 500;
		private const int MaxTimeInSeconds = 120;

		private WavePhysicsService _firstWaveService;
		private WavePhysicsService _secondWaveService;
		private IWavePhysicsService _interferenceService;

		public InterferenceTableService(WaveInfo firstWave, WaveInfo secondWave)
		{
			_firstWaveService = new WavePhysicsService(firstWave);
			_secondWaveService = new WavePhysicsService(secondWave);
			_interferenceService = new WaveInterferencePhysicsService(_firstWaveService, _secondWaveService);
		}

		public InterferenceValuesTableDialogViewModel Owner { get; set; }

		public IEnumerable<InterferenceTableRow> CalculateTable(float timeInterval)
		{
			if (Owner == null)
			{
				return Array.Empty<InterferenceTableRow>();
			}

			var time = Owner.Time;
			var distanceInterval = Owner.DistanceInterval;

			var table = new List<InterferenceTableRow>();

			float x = -20f;

			while (x <= 20f)
			{
				var first = _firstWaveService.CalculateY(x, time);
				var second = _secondWaveService.CalculateY(x, time);
				var interference = _interferenceService.CalculateY(x, time);
				var valuesRow = new InterferenceTableRow(x, first, second, interference);
				table.Add(valuesRow);
				x += distanceInterval;
			}

			return table;
		}
	}
}
