using System;
using System.Diagnostics;
using Microsoft.Graphics.Canvas.UI;

namespace Physics.Shared.UI.Rendering
{
    public class SimulationTime
    {
        private readonly Stopwatch _stopwatch;
        private long _lastElapsedMilliseconds = 0;

        public SimulationTime()
        {
            _stopwatch = new Stopwatch();
        }

        internal void Start()
        {
            _stopwatch.Start();
        }

        internal void OnUpdateStarting()
        {
            var currentElapsed = _stopwatch.ElapsedMilliseconds;
            ElapsedTime = TimeSpan.FromMilliseconds(currentElapsed - _lastElapsedMilliseconds);
            _lastElapsedMilliseconds = currentElapsed;
            TotalTime = TimeSpan.FromMilliseconds(currentElapsed);
            UpdateCount++;
        }

        public TimeSpan TotalTime { get; private set; } = TimeSpan.Zero;

        public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

        public long UpdateCount { get; private set; }

        public float SimulationSpeed { get; set; } = 1.0f;

        public void Restart()
        {            
            _stopwatch.Restart();
            TotalTime = TimeSpan.Zero;
        }

        public void Rewind(float time)
        {
            TotalTime = TimeSpan.FromSeconds(Math.Max(0.0, TotalTime.TotalSeconds - time));
        }

        public void FastForward(float time)
        {
            TotalTime = TimeSpan.FromSeconds(Math.Max(0.0, TotalTime.TotalSeconds + time));
        }

        internal void UpdateFromCanvas(CanvasTimingInformation updateTimingInformation)
        {
            ElapsedTime = updateTimingInformation.ElapsedTime * SimulationSpeed;
            TotalTime += ElapsedTime;
            UpdateCount = updateTimingInformation.UpdateCount;
        }
    }
}
