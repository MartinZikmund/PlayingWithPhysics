using System;
using System.Diagnostics;
using Microsoft.Graphics.Canvas.UI;

namespace Physics.Shared.UI.Rendering
{
    public class SimulationTime
    {
        private readonly Stopwatch _stopwatch;
        private readonly ICanvasController _canvasController;
        private long _lastElapsedMilliseconds = 0;

        public SimulationTime(ICanvasController canvasController)
        {
            _stopwatch = new Stopwatch();
            _canvasController = canvasController;
        }

        internal void Start()
        {
            _stopwatch.Start();
			_lastElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

		internal void Pause()
		{
			_stopwatch.Stop();
		}

        internal void OnUpdateStarting()
        {
            var currentElapsed = _stopwatch.ElapsedMilliseconds;
			var elapsedTime = (currentElapsed - _lastElapsedMilliseconds) * SimulationSpeed;
			var truncatedMilliseconds = TimeSpan.FromMilliseconds(Math.Truncate(elapsedTime));
			ElapsedTime = truncatedMilliseconds;
			_lastElapsedMilliseconds = currentElapsed;
			TotalTime += ElapsedTime;
            UpdateCount++;
        }		

        public TimeSpan TotalTime { get; private set; } = TimeSpan.Zero;

        public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

        public long UpdateCount { get; private set; }

        public float SimulationSpeed { get; set; } = 1.0f;

		public void Reset()
		{
			_stopwatch.Reset();
			TotalTime = TimeSpan.Zero;
			UpdateCount = 0;
			_lastElapsedMilliseconds = 0;
		}

		public void Restart()
		{
			_stopwatch.Restart();
            TotalTime = TimeSpan.Zero;
			UpdateCount = 0;
			_lastElapsedMilliseconds = 0;
        }

        public void Rewind(float time)
        {
            TotalTime = TimeSpan.FromSeconds(Math.Max(0.0, TotalTime.TotalSeconds - time));
        }

        public void FastForward(float time)
        {
            TotalTime = TimeSpan.FromSeconds(Math.Min(TotalTime.TotalSeconds + time, _canvasController.MaxTime?.TotalSeconds ?? double.MaxValue));
        }

        internal void UpdateFromCanvas(CanvasTimingInformation updateTimingInformation)
        {
            ElapsedTime = updateTimingInformation.ElapsedTime * SimulationSpeed;
            TotalTime = TimeSpan.FromSeconds(Math.Min(TotalTime.TotalSeconds + ElapsedTime.TotalSeconds, _canvasController.MaxTime?.TotalSeconds ?? double.MaxValue));
            UpdateCount++;
        }
    }
}
