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
		private WaveInterferencePhysicsService _interferenceService;

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

			var (firstMinX, firstMaxX) = GetWaveBounds(_firstWaveService);
			var (secondMinX, secondMaxX) = GetWaveBounds(_secondWaveService);
			var minX = Math.Min(firstMinX, secondMinX);
			var maxX = Math.Max(firstMaxX, secondMaxX);
			float x = minX;

			while (x <= maxX)
			{
				var first = _firstWaveService.CalculateY(x, time);
				var second = _secondWaveService.CalculateY(x, time);
				var interference = _interferenceService.CalculateY(x, time);
				var valuesRow = new InterferenceTableRow(x, first, second, interference);
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

		private (float minX, float maxX) GetWaveBounds(WavePhysicsService physicsService)
		{
			float minX = Math.Max(physicsService.StartX, physicsService.Wave.OriginX - physicsService.Wave.WaveLength * 3);
			float maxX = Math.Min(physicsService.EndX, physicsService.Wave.OriginX + physicsService.Wave.WaveLength * 3);
			return (minX, maxX);
		}
	}
}
